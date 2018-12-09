using System;
using System.Collections.Generic;

namespace BrettSpielMeister.Model
{
    public class Game
    {
        public List<Player> Players = new List<Player>();

        public Game(Map map)
        {
            Map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public Map Map { get; set; }
    }
}