using System.Collections.Generic;

namespace BrettSpielMeister.Model
{
    public class Game
    {
        public List<Player> Players = new List<Player>();

        public Map Map;

        public Game(Map map)
        {

        }
    }
}