using System;
using System.Collections.Generic;
using System.Text;
using BrettSpielMeister.Model;

namespace BrettSpielMeister.Logic.Rules
{
    public class RoundRobinPlayerSelection
    {
        private readonly GameLogic _gameLogic;

        private int _currentPlayer = 0;

        public RoundRobinPlayerSelection(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
        }

        public PlayerSet GetNextPlayer()
        {
            if (_gameLogic.Game.Players.Count == 0)
            {
                throw new InvalidOperationException("No player is given");
            }

            var result = _gameLogic.PlayerSets[_currentPlayer];
            _currentPlayer = (_currentPlayer + 1) % _gameLogic.PlayerSets.Count;

            _gameLogic.GetGameState().CurrentPlayer = result.Player;
            return result;
        }
    }
}
