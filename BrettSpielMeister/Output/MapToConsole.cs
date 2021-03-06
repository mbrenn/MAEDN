﻿using System;
using System.Linq;
using BrettSpielMeister.Model;
using BurnSystems.Logging;

namespace BrettSpielMeister.Output
{
    /// <summary>
    ///     Writes the current map to the console
    /// </summary>
    public class MapToConsole
    {
        private static readonly ILogger ClassLogger =new ClassLogger(typeof(MapToConsole));

        /// <summary>
        ///     Writes the map to the console by going through each field and writing the characters
        /// </summary>am>
        /// <param name="game">Game to be shown</param>
        /// <param name="logLevel">Log Level to be evaluated</param>
        public void Write(Game game, LogLevel logLevel = LogLevel.Debug)
        {
            var map = game.Map;
            if (map == null) throw new InvalidOperationException("map == null");
            if (map.Fields == null) throw new InvalidOperationException("map.Fields == null");

            var maxX = map.Fields.Max(x => x.X) + 1;
            var maxY = map.Fields.Max(x => x.Y) + 1;

            var characters = new char[maxY][];
            for (var n = 0; n < maxY; n++) characters[n] = new char[maxX];

            // Go through the fields to build up map
            foreach (var field in map.Fields)
            {
                var character = field.FieldType?.Character ?? 'x';
                characters[field.Y][field.X] = character;
            }

            // Go through the players and let them set their fields
            foreach (var player in game.Players)
            {
                var playerCharacter = player.Character;
                playerCharacter = playerCharacter == '0' ? '!' : playerCharacter;

                foreach (var figure in player.Figures)
                {
                    if (figure.Field == null) continue;

                    characters[figure.Field.Y][figure.Field.X] = playerCharacter;
                }
            }

            // Now perform the actual rendering
            for (var n = 0; n < maxY; n++)
            {
                var text = characters[n].Aggregate(string.Empty,
                    (target, item) => target + (item == 0 ? ' ' : item).ToString());
                ClassLogger.Log(logLevel, text);
            }
        }
    }
}