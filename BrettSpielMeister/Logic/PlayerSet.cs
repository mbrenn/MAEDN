using BrettSpielMeister.Model;
using BrettSpielMeister.States;

namespace BrettSpielMeister.Logic
{
    public class PlayerSet
    {
        public PlayerSet(Player player, PlayerState state, PlayerBehavior behavior)
        {
            Player = player;
            Behavior = behavior;
            State = state;
        }

        public Player Player { get; set; }
        public PlayerBehavior Behavior { get; set; }
        public PlayerState State { get; set; }
    }
}