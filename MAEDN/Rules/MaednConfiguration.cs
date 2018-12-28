using System;
using BrettSpielMeister.Logic;

namespace MAEDN.Rules
{
    public class MaednConfiguration : GameConfiguration
    {
        private int _numberOfPlayers = 2;

        /// <summary>
        /// Gets or sets a value indicating whether the game is a single player game
        /// </summary>
        public bool IsSinglePlayerGame
        {
            get => _numberOfPlayers == 1;
            set => _numberOfPlayers = value ? 1 : 2;
        }

        /// <summary>
        /// Gets or sets the number of players within the game
        /// </summary>
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

        /// <summary>
        /// Checks, whether the the last figure on the starting field is a blocker.
        /// That means, it may not be thrown. If an opponents figure gets onto this figure,
        /// the opponent will move back to home. 
        /// </summary>
        public bool WithBlocker { get; set; }
    }
}