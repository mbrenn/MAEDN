using System;
using System.Linq;
using BrettSpielMeister.Model;

namespace BrettSpielMeister.Output
{
    /// <summary>
    /// Writes the current map to the console
    /// </summary>
    public class MapToConsole
    {
        /// <summary>
        /// Writes the map to the console by going through each field and writing the characters
        /// </summary>
        /// <param name="map"></param>
        public void Write(Map map)
        {
            var maxX = map.Fields.Max(x => x.X) + 1;
            var maxY = map.Fields.Max(x => x.Y) + 1;

            var characters = new char[maxY][];
            for (var n = 0; n < maxY; n++)
            {
                characters[n] = new char[maxX];
            }

            foreach (var field in map.Fields)
            {
                var character = field.FieldType?.Character ?? 'x';
                characters[field.Y][field.X] = character;
            }

            for (var n = 0; n < maxY; n++)
            {
                var text = characters[n].Aggregate(string.Empty, (target, item) =>  target + item.ToString());
                Console.WriteLine(text);
            }
        }
    }
}
