using System;

namespace BrettSpielMeister.Logic
{
    public class Dice
    {
        public Random Random;

        public Dice(int maxNumber = 6)
        {
            MaxNumber = maxNumber;
            Random = new Random();
        }

        public int MaxNumber { get; set; }

        public int ThrowDice()
        {
            return Random.Next(MaxNumber) + 1;
        }
    }
}