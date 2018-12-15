using System;
using BrettSpielMeister.Logic;

namespace MAEDN.Rules
{
    public class MaednConfiguration : GameConfiguration
    {
        private int _numberOfPlayers = 2;

        public int NumberOfPlayers
        {
            get => _numberOfPlayers;
            set
            {
                if (_numberOfPlayers < 2 || _numberOfPlayers > 4)
                {
                    throw new ArgumentException("Player count must be between 2 and 4");
                }


                _numberOfPlayers = value;
            }
        }
    }
}