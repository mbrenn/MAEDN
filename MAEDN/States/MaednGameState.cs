using BrettSpielMeister.States;

namespace MAEDN.States
{
    public class MaednGameState : GameState
    {
        public DiceState? DiceState { get; set; }
    }
}