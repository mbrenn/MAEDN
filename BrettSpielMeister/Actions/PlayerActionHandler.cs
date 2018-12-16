using BrettSpielMeister.Logic;
using BrettSpielMeister.Model;

namespace BrettSpielMeister.Actions
{
    /// <summary>
    /// Defines a rule that allows the execution of an action. 
    /// This class is used in context of GameLogic. 
    /// </summary>
    public abstract class PlayerActionHandler
    {
        /// <summary>
        /// Invokes the action 
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="player"></param>
        /// <param name="action"></param>
        public abstract void Invoke(GameLogic logic, Player player, PlayerAction action);
    }
}