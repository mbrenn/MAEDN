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

        public override void InvokePlayerAction(PlayerAction action)
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            var allStartFields = new[]
                {Map.RedStartFields, Map.YellowStartFields, Map.BlueStartFields, Map.GreenStartFields};
            var playerCharacter = new[] {'r', 'y', 'b', 'g'};

            for (var n = 0; n < _configuration.NumberOfPlayers; n++)
            {
                var player = new Player
                {
                    Character = playerCharacter[n]
                };

                var startFields = allStartFields[n];
                foreach (var field in startFields)
                {
                    var figure = new Figure {Field = field};
                    player.Figures.Add(figure);
                }

                AddPlayer(player, new MaednPlayerState());
                Game.Players.Add(player);
            }
        }

        public override GameState GetGameState()
        {
            return _gameState;
        }

        public override void DoRound()
        {
            var nextPlayer = _playerSelection.GetNextPlayer();
            var playerBehavior = new DefaultBehavior(nextPlayer);
            playerBehavior.PerformTurm(this);
        }
    }

    public class MaednPlayerState : PlayerState
    {
    }
}