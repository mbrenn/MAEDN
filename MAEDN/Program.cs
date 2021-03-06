﻿using System;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using MAEDN.Rules;

namespace MAEDN
{
    internal class Program
    {
        private static void Main()
        {
            // Initializes the log
            TheLog.FilterThreshold = LogLevel.Trace;
            TheLog.AddProvider(new ConsoleProvider(), LogLevel.Error);
            TheLog.AddProvider(new FileProvider("maednlog.txt", true));

            for (var n = 0; n < 1000; n++)
            {
                // Runs the game
                var gameLogic = new MaednLogic(new MaednConfiguration
                {
                    NumberOfPlayers = 4
                });
                gameLogic.Run();
            }

            TheLog.Fatal("Done");
            Console.ReadKey();
        }
    }
}