using System;
using System.Data;
using System.Linq;
using BrettSpielMeister.Actions;
using BrettSpielMeister.Logic;
using BrettSpielMeister.Logic.Rules;
using BrettSpielMeister.Model;
using BrettSpielMeister.States;
using MAEDN.Behaviors;
using MAEDN.States;

namespace MAEDN.Rules
{
    public class MaednLogic : GameLogic
    {
        private readonly RoundRobinPlayerSelection _playerSelection;

        private readonly MaednConfiguration _configuration;

        private readonly MaednGameState _gameState;

        public override GameState GameState => _gameState;

        public new MaednGame Game => (MaednGame)base.Game;

        public MaednMap Map => (MaednMap)Game.Map;

        public MaednLogic(MaednConfiguration configuration) : base(configuration, new MaednGame())
        {
            _configuration = configuration;
            _gameState = new MaednGameState();
            _playerSelection = new RoundRobinPlayerSelection(this);;
        }
        
        /// <summary>
        /// Initializes the figures
        /// </summary>
        public override void Setup()
        {
            // Initializes the rule
            _gameState.DiceState = new DiceState();
            AddRule(x => x is DiceAction, new DiceActionHandler(new Dice( _gameState.DiceState)));

            // Initializes the players
            var allHomeFields = new[]
                {Map.RedHomeFields, Map.YellowHomeFields, Map.BlueHomeFields, Map.GreenHomeFields};
            var playerCharacter = new[] {'r', 'y', 'b', 'g'};
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
            UpdatePlayerState(currentPlayer);
            currentPlayerState.DicesInThisRound = 0;
            
            // Gives information to console
            Console.WriteLine($"{currentPlayer} turn");
            currentPlayerState.ToConsole();

            while (true)
            {
                EvaluateTurnState();
                if (TurnState is TurnFinishTurnState)
                {
                    break;
                }

                var action = currentPlayer.Behavior.PerformTurn(this);
                Console.WriteLine($"Chosen action: {action}");

                if (!EvaluateValidity(action))
                {
                    throw new InvalidOperationException("Cheating detected");
                }

                InvokePlayerAction(currentPlayer.Player, action);
            }
        }

        /// <summary>
        /// Evaluates the possible turn state of the game.
        /// Dependent on whether the user already has diced or not 
        /// </summary>
        private void EvaluateTurnState()
        {
            if (_gameState.DiceState.IsDiced)
            {
                TurnState = new TurnMoveFigureState();
            }
            else
            {
                TurnState = new TurnDiceState();
            }
        }

        /// <summary>
        /// Evaluates whether the current action is a valid action
        /// </summary>
        /// <param name="action">Action to be performed</param>
        /// <returns>True, if the action is valid, otherwise not</returns>
        private bool EvaluateValidity(PlayerAction action)
        {
            if (TurnState is TurnDiceState)
            {
                if (action is DiceAction)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks, if the figure is in home 
        /// </summary>
        /// <param name="playerSet">Player to be queried</param>
        /// <param name="figure">Figure to be queried</param>
        /// <returns>True, if figure is in home</returns>
        public static bool IsFigureInHome(PlayerSet playerSet, Figure figure)
        {
            return ((MaednPlayerState) (playerSet.State)).HomeFields.Contains(figure.Field);
        }

        /// <summary>
        /// Checks, if the figure is on startfield of player
        /// </summary>
        /// <param name="playerSet">Player to be queried</param>
        /// <param name="figure">Figure to be queried</param>
        /// <returns>True, if figure is in startfield</returns>
        public static bool IsFigureInStart(PlayerSet playerSet, Figure figure)
        {
            return ((MaednPlayerState)(playerSet.State)).StartField == figure.Field;
        }

        /// <summary>
        /// Checks, if the figure is in home 
        /// </summary>
        /// <param name="playerSet">Player to be queried</param>
        /// <param name="figure">Figure to be queried</param>
        /// <returns>True, if figure is in home</returns>
        public static bool IsFigureInGoal(PlayerSet playerSet, Figure figure)
        {
            return ((MaednPlayerState)(playerSet.State)).GoalFields.Contains(figure.Field);
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
    }
}