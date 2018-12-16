using BrettSpielMeister.Logic;
using BrettSpielMeister.Model;

namespace BrettSpielMeister.Actions
{
    public class DiceActionHandler : PlayerActionHandler
    {
        public DiceActionHandler(Dice dice)
        {
            Dice = dice;
        }

        public Dice Dice { get; } 

        /// <summary>
        /// Throws the dice
        /// </summary>
        /// <param name="logic">Logic of the game</param>
        /// <param name="player">Data of the player</param>
        /// <param name="action">Action player</param>
        public override void Invoke(GameLogic logic, Player player, PlayerAction action)
        {
            Dice.ThrowDice();
        }
    }
}