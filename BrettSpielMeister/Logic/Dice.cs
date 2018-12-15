using System;
using BrettSpielMeister.States;

namespace BrettSpielMeister.Logic
{
    public class Dice
    {
        public Random Random;

        private DiceState _diceState;

        public Dice(DiceState diceState, int maxNumber = 6)
        {
            _diceState = diceState;
            MaxNumber = maxNumber;
            Random = new Random();
        }

        public int MaxNumber { get; set; }

        public void ThrowDice()
        {
            if (_diceState.IsDiced)
            {
                throw new InvalidOperationException("Dice is already diced");
            }

            _diceState.IsDiced = true;
            _diceState.CurrentDiceValue = Random.Next(MaxNumber) + 1;
        }

        public void PickupDice()
        {
            _diceState.IsDiced = false;
        }
    }
}