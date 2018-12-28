using System;
using System.Collections.Generic;
using System.Text;
using BrettSpielMeister.Model;
using BurnSystems.Logging;

namespace BrettSpielMeister.Logic.Rules
{
    public class RoundRobinPlayerSelection
    {
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(RoundRobinPlayerSelection)); 

        private readonly GameLogic _gameLogic;

        private int _currentPlayer = 0;

        public RoundRobinPlayerSelection(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
        }

        /// <summary>
        /// Gets the next player
        /// </summary>
        /// <returns>Defines the player, who is next in charge</returns>
        public PlayerSet GetNextPlayer()
        {
            if (_gameLogic.Game.Players.Count == 0)
            {
                ClassLogger.Error("No player is given");
                throw new InvalidOperationException("No player is given");
            }

            var result = _gameLogic.PlayerSets[_currentPlayer];
            _currentPlayer = (_currentPlayer + 1) % _gameLogic.PlayerSets.Count;

            _gameLogic.GameState.CurrentPlayer = result.Player;
            return result;
        }
    }
}
