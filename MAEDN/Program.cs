using System;
using BrettSpielMeister.Output;
using MAEDN.Rules;

namespace MAEDN
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gameLogic = new MaednLogic(new MaednConfiguration());
            gameLogic.Run();

            var mapToConsole = new MapToConsole();
            mapToConsole.Write(gameLogic.Game);

            Console.ReadKey();
        }
    }
}