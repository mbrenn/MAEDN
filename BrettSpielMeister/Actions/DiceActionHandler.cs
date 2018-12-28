using BrettSpielMeister.Logic;
using BrettSpielMeister.Model;
using BurnSystems.Logging;

namespace BrettSpielMeister.Actions
{
    public class DiceActionHandler : PlayerActionHandler
    {
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(DiceActionHandler));
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
            ClassLogger.Debug($"Dice thrown: {Dice.DiceState.CurrentDiceValue}");
        }
    }
}