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

        public Player GetNextPlayer()
        {
            if (_gameLogic.Game.Players.Count == 0)
            {
                throw new InvalidOperationException("No player is given");
            }

            var result = _gameLogic.Game.Players[_currentPlayer];
            _currentPlayer = _currentPlayer % _gameLogic.Game.Players.Count;

            return result;
        }
    }
}
