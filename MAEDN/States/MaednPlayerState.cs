using System.Collections.Generic;
using BrettSpielMeister.Model;
using BrettSpielMeister.States;
using BurnSystems.Logging;

namespace MAEDN.States
{
    /// <summary>
    /// Defines the state of the player. Will be calculated each time, a player is in turn
    /// </summary>
    public class MaednPlayerState : PlayerState
    {
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(MaednPlayerState));

        public MaednPlayerState(Field startField, IEnumerable<Field> homeFields, IEnumerable<Field> goalFields)
        {
            StartField = startField;
            HomeFields = homeFields;
            GoalFields = goalFields;
        }

        public int DicesInThisRound { get; set; }

        public bool IsCompletelyInHome { get; set; }

        public bool IsCompletelyInGoal { get; set; }

        public bool HasBlocker { get; set; }

        /// <summary>
        /// Gets or sets the information whether the player may dice triple
        /// This is the case, when all the goal figures are on the very last position
        /// and all the other figures are on home position.
        /// </summary>
        public bool MayDiceTriple { get; set; }

        public Field StartField { get; }

        public bool IsLastDiceASix { get; }

        public IEnumerable<Field> HomeFields { get; }

        public IEnumerable<Field> GoalFields { get; }

        public void ToLogger()
        {
            ClassLogger.Trace($"Has Won   : {HasWon}, Has Lost  : {HasLost}, In Home   : {IsCompletelyInHome}");
            ClassLogger.Trace($"In Goal   : {IsCompletelyInGoal}, Dice 3x   : {MayDiceTriple}, In Blocker: {HasBlocker}");
            ClassLogger.Trace($"Last Dice6: {IsLastDiceASix}");
        }
    }
}