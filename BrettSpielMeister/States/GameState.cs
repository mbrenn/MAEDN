using BrettSpielMeister.Model;

namespace BrettSpielMeister.States
{
    public class GameState
    {
        public bool IsFinished { get; set; }

        /// <summary>
        /// Gets or sets the player, who is currently in charge of the next action
        /// </summary>
        public Player CurrentPlayer { get; set; }
    }
}