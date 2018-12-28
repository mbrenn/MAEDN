using System;
using BrettSpielMeister.Output;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using MAEDN.Rules;

namespace MAEDN
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Initializes the log
            TheLog.FilterThreshold = LogLevel.Trace;
            TheLog.AddProvider(new ConsoleProvider(), LogLevel.Debug);
            TheLog.AddProvider(new FileProvider("maednlog.txt", true), LogLevel.Trace);
            
            // Runs the game
            var gameLogic = new MaednLogic(new MaednConfiguration()
            {
                NumberOfPlayers = 4
            });
            gameLogic.Run();

            var mapToConsole = new MapToConsole();
            mapToConsole.Write(gameLogic.Game, LogLevel.Info);

            Console.ReadKey();
        }
    }
}