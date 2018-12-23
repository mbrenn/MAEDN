using BrettSpielMeister.Actions;

namespace BrettSpielMeister.Logic
{
    public abstract class PlayerBehavior
    {
        /// <summary>
        /// Called when the player is requested to perform a turn
        /// </summary>
        /// <param name="gameLogic">Game Logic being used</param>
        public abstract PlayerAction PerformTurn(GameLogic gameLogic);
    }
}