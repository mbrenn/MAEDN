using BrettSpielMeister.Model;

namespace MAEDN.Rules
{
    public class MaednGame : Game
    {
        public MaednGame() :
            base(new MaednMap())
        {
        }
    }
}