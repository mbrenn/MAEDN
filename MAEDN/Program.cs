using System;
using BrettSpielMeister.Output;
using MAEDN.Rules;

namespace MAEDN
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new MaednMap();
            map.Create();

            var mapToConsole = new MapToConsole();
            mapToConsole.Write(map);

            Console.ReadKey();
        }
    }
}
