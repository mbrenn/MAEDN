using System.Collections.Generic;

namespace BrettSpielMeister.Model
{
    public class Player
    {
        public string Name { get; set; }

        public List<Figure> Figures = new List<Figure>();

        public override string ToString()
        {
            return Name;
        }
    }
}