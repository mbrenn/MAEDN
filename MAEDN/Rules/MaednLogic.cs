using BrettSpielMeister.Logic;
using BrettSpielMeister.Model;

namespace MAEDN.Rules
{
    public class MaednLogic : GameLogic
    {
        private readonly MaednConfiguration _configuration;

        public MaednLogic(MaednConfiguration configuration) : base(new MaednGame())
        {
            _configuration = configuration;
        }

        public new MaednGame Game => (MaednGame) base.Game;
        public MaednMap Map => (MaednMap) Game.Map;

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

                Game.Players.Add(player);
            }
        }
    }
}