using System;

namespace BrettSpielMeister.Logic
{
    public class Dice
    {
        public Random Random;
        public int MaxNumber { get; set; }

        public Dice(int maxNumber = 6)
        {
            MaxNumber = maxNumber;
            Random = new Random();
        }

        public int ThrowDice()
        {
            return Random.Next(MaxNumber) + 1;
        }
    }
}