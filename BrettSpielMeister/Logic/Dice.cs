using System;
using BrettSpielMeister.States;

namespace BrettSpielMeister.Logic
{
    public class Dice
    {
        public Random Random;

        public DiceState DiceState { get; }

        public Dice(DiceState diceState, int maxNumber = 6)
        {
            DiceState = diceState;
            MaxNumber = maxNumber;
            Random = new Random();
        }

        public int MaxNumber { get; set; }

        public void ThrowDice()
        {
            if (DiceState.IsDiced)
            {
                throw new InvalidOperationException("Dice is already diced");
            }

            DiceState.IsDiced = true;
            DiceState.CurrentDiceValue = Random.Next(MaxNumber) + 1;
        }

        public void PickupDice()
        {
            DiceState.IsDiced = false;
        }
    }
}