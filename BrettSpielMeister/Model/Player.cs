using System.Collections.Generic;

namespace BrettSpielMeister.Model
{
    public class Player
    {
        public List<Figure> Figures = new List<Figure>();
        public string? Name { get; set; }

        public char Character { get; set; }

        public override string ToString()
        {
            return Name ?? "No name";
        }
    }
}