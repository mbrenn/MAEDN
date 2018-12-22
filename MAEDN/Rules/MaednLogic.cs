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

        public MaednLogic(MaednConfiguration configuration) : base(configuration, new MaednGame())
        {
            _configuration = configuration;
            _gameState = new MaednGameState();
            _playerSelection = new RoundRobinPlayerSelection(this);;
        }

        public new MaednGame Game => (MaednGame) base.Game;

        public MaednMap Map => (MaednMap) Game.Map;
        
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
                {Map.RedHomeFields, Map.YellowHomeFields, Map.BlueHomeFields, Map.GreenStartFields};
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
                    new DefaultBehavior(player));
            }
        }

        public override GameState GetGameState()
        {
            return _gameState;
        }

        public void UpdatePlayerState(PlayerSet set)
        {
            UpdatePlayerState(set.Player, set.State);
        }

        public override void UpdatePlayerState(Player player, PlayerState playerState)
        {
            var maednPlayerState = (MaednPlayerState) playerState;
            // Check, whether player has all persons in home
            var allIn = true;
            foreach (var figure in player.Figures)
            {
                if (!maednPlayerState.HomeFields.Contains(figure.Field))
                {
                    allIn = false;
                }
            }

            maednPlayerState.IsCompletelyInHome = allIn;

            // Check, whether player has all persons in home
            allIn = true;
            foreach (var figure in player.Figures)
            {
                if (!maednPlayerState.GoalFields.Contains(figure.Field))
                {
                    allIn = false;
                }
            }

            maednPlayerState.IsCompletelyInGoal = allIn;
        }

        /// <summary>
        /// Executes the round
        /// </summary>
        public override void DoRound()
        {
            var currentPlayer = _playerSelection.GetNextPlayer();
            var currentPlayerState = (MaednPlayerState) currentPlayer.State;
            UpdatePlayerState(currentPlayer);
            currentPlayerState.DicesInThisRound = 0;
            
            // Gives information to console
            Console.WriteLine($"{currentPlayer} turn");
            currentPlayerState.ToConsole();
            
            TurnState = new TurnDiceState();

            var action = currentPlayer.Behavior.PerformTurm(this);
            Console.WriteLine($"Chosen action: {action}");
            if (!EvaluateValidity(action))
            {
                throw new InvalidOperationException("Cheating detected");
            }

            InvokePlayerAction(currentPlayer.Player, action);
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
    }
}