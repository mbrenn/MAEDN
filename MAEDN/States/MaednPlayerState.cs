using BrettSpielMeister.States;

namespace MAEDN.Rules
{
    /// <summary>
    /// Defines the state of the player. Will be calculated each time, a player is in turn
    /// </summary>
    public class MaednPlayerState : PlayerState
    {
        public bool IsCompletelyInHome { get; set; }

        public bool IsCompletelyInGoal { get; set; }

        public bool HasBlockingHomeFunction { get; set; }
    }
}