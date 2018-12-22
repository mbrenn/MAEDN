using System;
using System.Collections.Generic;
using BrettSpielMeister.Model;
using BrettSpielMeister.States;

namespace MAEDN.Rules
{
    /// <summary>
    /// Defines the state of the player. Will be calculated each time, a player is in turn
    /// </summary>
    public class MaednPlayerState : PlayerState
    {
        public MaednPlayerState(Field startField, IEnumerable<Field> homeFields, IEnumerable<Field> goalFields)
        {
            StartField = startField;
            HomeFields = homeFields;
            GoalFields = goalFields;
        }

        public int DicesInThisRound { get; set; }

        public bool IsCompletelyInHome { get; set; }

        public bool IsCompletelyInGoal { get; set; }

        public bool HasBlockingHomeFunction { get; set; }

        /// <summary>
        /// Gets or sets the information whether the player may dice triple
        /// This is the case, when all the goal figures are on the very last position
        /// and all the other figures are on home position.
        /// </summary>
        public bool MayDiceTriple { get; set; }

        public Field StartField { get; }

        public IEnumerable<Field> HomeFields { get; }

        public IEnumerable<Field> GoalFields { get; }

        public void ToConsole()
        {
            Console.WriteLine($"Has Won   : {HasWon}");
            Console.WriteLine($"Has Lost  : {HasLost}");
            Console.WriteLine($"In Home   : {IsCompletelyInHome}");
            Console.WriteLine($"In Goal   : {IsCompletelyInGoal}");
            Console.WriteLine($"Dice 3x   : {MayDiceTriple}");
            Console.WriteLine($"In Blocker: {HasBlockingHomeFunction}");
        }
    }
}