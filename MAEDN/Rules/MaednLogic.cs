using System;
using System.Collections.Generic;
using System.Linq;
using BrettSpielMeister.Actions;
using BrettSpielMeister.Logic;
using BrettSpielMeister.Logic.Rules;
using BrettSpielMeister.Model;
using BrettSpielMeister.Output;
using BrettSpielMeister.States;
using BurnSystems.Logging;
using MAEDN.Behaviors;
using MAEDN.States;

namespace MAEDN.Rules
{
    public class MaednLogic : GameLogic
    {
        private readonly ILogger ClassLogger = new ClassLogger(typeof(MaednLogic));

        private readonly RoundRobinPlayerSelection _playerSelection;

        private readonly MaednConfiguration _configuration;

        private readonly MaednGameState _gameState;

        public Dice Dice { get; private set; }

        public override GameState GameState => _gameState;

        public new MaednGame Game => (MaednGame)base.Game;

        public MaednMap Map => (MaednMap)Game.Map;

        public MaednLogic(MaednConfiguration configuration) : base(configuration, new MaednGame())
        {
            _configuration = configuration;
            _gameState = new MaednGameState();
            _playerSelection = new RoundRobinPlayerSelection(this);
        }
        
        /// <summary>
        /// Initializes the figures
        /// </summary>
        public override void Setup()
        {
            // Initializes the rule
            _gameState.DiceState = new DiceState();
            Dice = new Dice(_gameState.DiceState);

            AddRule(x => x is DiceAction, new DiceActionHandler(Dice));

            // Initializes the players
            var allHomeFields = new[]
                {Map.RedHomeFields, Map.YellowHomeFields, Map.BlueHomeFields, Map.GreenHomeFields};
            var playerCharacter = new[] {'R', 'Y', 'B', 'G'};
            var playerNames = new[] {"Red", "Yellow", "Blue", "Green"};
            var allGoalFields = new[]
                {Map.RedGoalFields, Map.YellowGoalFields, Map.BlueGoalFields, Map.GreenGoalFields};
            var allStartFields = new[]
                {Map.Fields[32], Map.Fields[42], Map.Fields[52], Map.Fields[62]};

            for (var n = 0; n < _configuration.NumberOfPlayers; n++)
            {
                var player = new Player
                {
                    Character = playerCharacter[n],
                    Name = playerNames[n]
                };

                var homeFields = allHomeFields[n];
                foreach (var field in homeFields)
                {
                    var figure = new Figure {Field = field};
                    player.Figures.Add(figure);
                }

                AddPlayer(
                    player, 
                    new MaednPlayerState(
                        allStartFields[n], 
                        allHomeFields[n],
                        allGoalFields[n]),
                    new DefaultBehavior(this, player));
            }
        }

        public override PlayerState UpdatePlayerState(PlayerSet playerSet)
        {
            var maednPlayerState = (MaednPlayerState) playerSet.State;
            var player = playerSet.Player;

            // Check, whether player has all persons in home
            maednPlayerState.IsCompletelyInHome = AreAllFiguresInHome(player, maednPlayerState);
            maednPlayerState.IsCompletelyInGoal = AreAllFiguresInGoal(player, maednPlayerState);
            maednPlayerState.MayDiceTriple = AreFiguresPressedInGoalOrHome(playerSet);
            maednPlayerState.HasWon = maednPlayerState.IsCompletelyInGoal;

            return maednPlayerState;
        }

        public static bool AreAllFiguresInGoal(Player player, MaednPlayerState maednPlayerState)
        {
            // Check, whether player has all persons in home
            foreach (var figure in player.Figures)
            {
                if (!maednPlayerState.GoalFields.Contains(figure.Field))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AreAllFiguresInHome(Player player, MaednPlayerState maednPlayerState)
        {
            foreach (var figure in player.Figures)
            {
                if (!maednPlayerState.HomeFields.Contains(figure.Field))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Executes the round
        /// </summary>
        public override void DoRound()
        {
            // Chooses the next player
            var currentPlayer = _playerSelection.GetNextPlayer();
            var currentPlayerState = (MaednPlayerState) currentPlayer.State;
            currentPlayerState.DicesInThisRound = 0;
            TurnState = new TurnDiceState();
            
            // Gives information to console
            while (true)
            {
                UpdatePlayerState(currentPlayer);

                ClassLogger.Debug($"{currentPlayer.Player} turn");
                currentPlayerState.ToLogger();
                
                if (TurnState is TurnFinishState)
                {
                    break;
                }

                var action = currentPlayer.Behavior.PerformTurn(this);
                ClassLogger.Debug($"Chosen action: {action}");

                
                switch (action)
                {
                    case DiceAction _ when TurnState is TurnDiceState:
                    {
                        // Ok, throw the dice
                        InvokePlayerAction(currentPlayer.Player, action);

                        if (currentPlayerState.MayDiceTriple)
                        {
                            // User cannot move any figure. Figures in starting point
                            // or pressed in goal 
                            if (_gameState.DiceState.CurrentDiceValue != 6)
                            {
                                // Player cannot move figure, but may dice again
                                currentPlayerState.DicesInThisRound++;
                                if (currentPlayerState.DicesInThisRound == 3)
                                {
                                    new Dice(_gameState.DiceState).PickupDice();
                                    TurnState = new TurnFinishState();
                                }
                                else
                                {
                                    // Revoke dice status and user may throw dice again
                                    new Dice(_gameState.DiceState).PickupDice();
                                    TurnState = new TurnDiceState();
                                }
                            }
                            else
                            {
                                TurnState = new TurnMoveFigureState();
                            }
                        }
                        else
                        {
                            // Player can move, will be decided in next action
                            TurnState = new TurnMoveFigureState();
                        }

                        break;
                    }
                    case DiceAction _: 
                        ClassLogger.Error("Dicing is not allowed");
                        throw new InvalidOperationException("Dicing is not allowed");

                    case MoveFigureAction moveAction when TurnState is TurnMoveFigureState:
                        // Do something
                        var valid = GetAllowedTurns().Any(
                            x => x.Figure == moveAction.Figure && x.TargetField == moveAction.TargetField);
                        if (!valid)
                        {
                            ClassLogger.Error("The chosen turn is now valid");
                            throw new InvalidOperationException("The chosen turn is now valid");
                        }

                        // Checks, if there is an opponent figure upon the field
                        var (otherPlayer, figure) = GetFigureOnField(moveAction.TargetField);
                        if (otherPlayer != null)
                        {
                            ClassLogger.Debug($"'{currentPlayer.Player.Name}' throws out '{otherPlayer.Player.Name}'");
                            // Bring him back to home, Muahahahaha
                            figure.Field = GetFreeHomeField(otherPlayer);
                        }

                        // Move figure
                        moveAction.Figure.Field = moveAction.TargetField;

                        Dice.PickupDice();

                        UpdatePlayerState(currentPlayer);
                        if (Dice.DiceState.CurrentDiceValue == 6 && !currentPlayer.GetMaednPlayerState().HasWon)
                        {
                            // User has diced a six, he may again
                            TurnState = new TurnDiceState();
                        }
                        else
                        {
                            // User has ended
                            TurnState = new TurnFinishState();
                        }

                        new MapToConsole().Write(Game);

                        break;

                    case MoveFigureAction _:
                        ClassLogger.Error("Moving is not allowed");
                        throw new InvalidOperationException("Moving is not allowed");

                    case DoNothingAction _:
                        Dice.PickupDice();
                        if (Dice.DiceState.CurrentDiceValue == 6)
                        {
                            // User has diced a six, he may again
                            TurnState = new TurnDiceState();
                        }
                        else
                        {
                            // User has ended
                            TurnState = new TurnFinishState();
                        }
                        break;

                    default:
                        ClassLogger.Error("Unknown action...");
                        throw new InvalidOperationException("Unknown action...");
                }
            }
        }

        /// <summary>
        /// Gets the figure being on the given field by the player
        /// </summary>
        /// <param name="playerSet">Playerset to be evaluated</param>
        /// <param name="field">Field to be queried</param>
        /// <returns>The given figure, if figure is on the field, otherwise null</returns>
        public static Figure GetFigureOnField(PlayerSet playerSet, Field field)
        {
            return playerSet.Player.Figures.FirstOrDefault(x => x.Field == field);
        }

        /// <summary>
        /// Gets the figure of the field, not concerned about any player association
        /// </summary>
        /// <param name="game">Game being used</param>
        /// <param name="field">Field to be used</param>
        /// <returns></returns>
        public (PlayerSet, Figure) GetFigureOnField(Field field)
        {
            foreach (var playerSet in PlayerSets)
            {
                var foundFigure = playerSet.Player.Figures.FirstOrDefault(x => x.Field == field);
                if (foundFigure != null)
                {
                    return (playerSet, foundFigure);
                }
            }

            return (null,null);
        }

        /// <summary>
        /// Checks, if the figure is in home 
        /// </summary>
        /// <param name="playerSet">Player to be queried</param>
        /// <param name="figure">Figure to be queried</param>
        /// <returns>True, if figure is in home</returns>
        public static bool IsFigureInHome(PlayerSet playerSet, Figure figure)
        {
            return ((MaednPlayerState) playerSet.State).HomeFields.Contains(figure.Field);
        }

        /// <summary>
        /// Checks, whether the player has any figure in the home
        /// </summary>
        /// <param name="playerSet">Player set to be evaluated</param>
        /// <returns>true, if any of the figure is on home</returns>
        public static bool IsAnyFigureInHome(PlayerSet playerSet)
        {
           return playerSet.Player.Figures.Any(
                figure => playerSet.State.GetMaednPlayerState()
                    .HomeFields.Contains(figure.Field));

        }

        /// <summary>
        /// Checks, if the figure is on startfield of player
        /// </summary>
        /// <param name="playerSet">Player to be queried</param>
        /// <param name="figure">Figure to be queried</param>
        /// <returns>True, if figure is in startfield</returns>
        public static bool IsFigureInStart(PlayerSet playerSet, Figure figure)
        {
            return ((MaednPlayerState)playerSet.State).StartField == figure.Field;
        }

        /// <summary>
        /// Checks whether the player has a figure on the starting field (block)
        /// </summary>
        /// <param name="player">Player to be evaluated</param>
        /// <returns>true, if the player has figure on the starting field</returns>
        public static bool HasFigureOnStartingField(PlayerSet player)
        {
            return HasFigureOnField(player, ((MaednPlayerState) player.State).StartField);
        }
        
        /// <summary>
        /// Checks, whether the given figure is a blocker. 
        /// </summary>
        /// <param name="playerSet">Player set to be evaluated</param>
        /// <param name="figure">Figure of the player being checked</param>
        /// <returns>true, if the figure is a blocker</returns>
        public static bool IsFigureABlocker(PlayerSet playerSet, Figure figure)
        {
            if (figure.Field != ((MaednPlayerState) playerSet.State).StartField)
            {
                return false;
            }

            if (IsAnyFigureInHome(playerSet))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks, if the figure is in home 
        /// </summary>
        /// <param name="playerSet">Player to be queried</param>
        /// <param name="figure">Figure to be queried</param>
        /// <returns>True, if figure is in home</returns>
        public static bool IsFigureInGoal(PlayerSet playerSet, Figure figure)
        {
            return ((MaednPlayerState)playerSet.State).GoalFields.Contains(figure.Field);
        }

        /// <summary>
        /// Gets the first free home field of the player or null, if there is no free home field
        /// </summary>
        /// <param name="playerSet">PlayerSet to be evaluated</param>
        /// <returns>The found field</returns>
        public static Field GetFreeHomeField(PlayerSet playerSet)
        {
            return playerSet.GetMaednPlayerState().HomeFields.FirstOrDefault(
                x => playerSet.Player.Figures.All(y => y.Field != x));
        }

        /// <summary>
        /// Gets the next appropriating field of the player
        /// </summary>
        /// <param name="player">Player to be evaluated</param>
        /// <param name="field">Field to be queried</param>
        /// <returns>The next field or null, if there is no next field</returns>
        public Field GetNextFieldForPlayer(PlayerSet player, Field field)
        {
            var movingFields = Map.MovingFields.ToList();
            var position = movingFields.IndexOf(field);
            if (position == -1)
            {
                // Field is not a moving field
                if (player == null)
                {
                    // No player, so player is on invalid field
                    return null;
                }
                else
                {
                    for (var n = 0; n < 4; n++)
                    {
                        var goalField = player.GetMaednPlayerState().GoalFields.ElementAt(n);
                        if (goalField == field)
                        {
                            if (n == 3)
                            {
                                // Player is already on last goal field
                                return null;
                            }

                            // Player is on a goal field and advances to the next field
                            return player.GetMaednPlayerState().GoalFields.ElementAt(n + 1);
                        }
                    }

                    // Player is not on moving fields and not on goal fields, so he
                    // must be on one of the starting fields or he has cheated
                    return null;
                }

            }

            // If player is on a moving field, advance to the next moving field
            position = (position + 1) % movingFields.Count;

            var foundField = movingFields[position];
            if (player == null)
            {
                return foundField;
            }

            // Player would be upon starting field.
            if (foundField == 
                player.GetMaednPlayerState().StartField)
            {
                foundField = player.GetMaednPlayerState().GoalFields.First();
            }

            return foundField;
        }

        /// <summary>
        /// Forwards a several steps of fields for a certain player
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="field"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public Field GetNextFieldForPlayer(PlayerSet player, Field field, int steps)
        {
            for (var n = 0; n < steps; n++)
            {
                field = GetNextFieldForPlayer(player, field);
                if (field == null)
                {
                    return null;
                }
            }

            return field;
        }

        /// <summary>
        /// Verifies whether the figures are pressed on goal (so, that no movement is possible)
        /// or whether the rest of the figures are in home.
        /// This means that the player may dice 3 times...
        /// </summary>
        /// <param name="set">Player set to be set</param>
        /// <returns>true, if the player is in home</returns>
        public static bool AreFiguresPressedInGoalOrHome(PlayerSet set)
        {
            // Count figures in home
            var homeCount = set.Player.Figures.Count(x => IsFigureInHome(set, x));
            var figureCount = set.Player.Figures.Count;
            var playerState = (MaednPlayerState) set.State;

            var restCount = figureCount - homeCount;

            for (var n = 0; n < restCount; n++)
            {
                var checkGoal = playerState.GoalFields.ElementAt(figureCount - n - 1);
                var onLastGoal = set.Player.Figures.Any(x => x.Field == checkGoal);
                if (!onLastGoal)
                {
                    return false;
                }
            }

            return true;
        }

        public List<AllowedTurn> GetAllowedTurns()
        {
            var result = new List<AllowedTurn>();
            if (Dice.DiceState.IsDiced == false)
            {
                // Not diced, no action
                return result;
            }

            var playerSet = GetPlayerSet(_gameState.CurrentPlayer);
            var playerState = (MaednPlayerState) playerSet.State;
            var hasFigureOnStartingField = HasFigureOnStartingField(playerSet);

            var isPlayerObligedToLeaveHouse =
                Dice.DiceState.CurrentDiceValue == 6 &&
                !hasFigureOnStartingField
                && IsAnyFigureInHome(playerSet);

            // Check, if user HAS to leave his house because a six is diced and
            // the starting field is free
            if (isPlayerObligedToLeaveHouse)
            {
                foreach (var figure in _gameState.CurrentPlayer.Figures)
                {
                    if (IsFigureInHome(playerSet, figure))
                    {
                        result.Add(
                            new AllowedTurn(figure, playerState.StartField));
                    }
                }

                return result;
            }

            // User can use any figure, as long as target field is not occupied by
            // himself
            foreach (var figure in _gameState.CurrentPlayer.Figures)
            {
                // Advance to the number of fields
                var targetField = GetNextFieldForPlayer(
                    playerSet, figure.Field, Dice.DiceState.CurrentDiceValue);
                if (targetField == null)
                {
                    // Figure is in advance
                    continue;
                }

                // Check, if player has occupied the field itself
                if (HasFigureOnField(playerSet, targetField))
                {
                    continue;
                }

                // Checks, if the player has a figure on the starting field, which is not the blocker
                // And checks whether this figure can move to the diced field. 
                if (figure.Field == playerState.StartField && !playerState.HasBlocker)
                {
                    result.Clear();
                    result.Add(new AllowedTurn(
                        figure,
                        targetField));

                    // This is the only allowed move... free the starting field
                    return result;

                }

                result.Add(new AllowedTurn(
                    figure,
                    targetField));
            }

            return result;
        }
    }
}