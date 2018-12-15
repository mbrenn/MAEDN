using System;
using System.Collections.Generic;
using BrettSpielMeister.Actions;
using BrettSpielMeister.Model;
using BrettSpielMeister.Output;
using BrettSpielMeister.States;

namespace BrettSpielMeister.Logic
{
    public abstract class GameLogic
    {
        private readonly GameConfiguration _gameConfiguration;
        private Dictionary<Player, PlayerState> _playerStates = new Dictionary<Player, PlayerState>();

        public GameLogic(GameConfiguration gameConfiguration, Game game)
        {
            _gameConfiguration = gameConfiguration;
            Game = game;
        }

        public PlayerState GetPlayerState(Player player)
        {
            if (_playerStates.TryGetValue(player, out var playerState))
            {
                return playerState;
            }

            return null;
        }

        public Game Game { get; }

        /// <summary>
        /// Resets the game 
        /// </summary>
        public void Reset()
        {
            Game.Players.Clear();
        }

        public void Run()
        {
            Game.Map.Create();

            Setup();
            // Add player states

            var currentRound = 0;

            do
            {
                var gameState = GetGameState();

                DoRound();
                currentRound++;

                Console.WriteLine($"Round {currentRound} done.");
                new MapToConsole().Write(Game);

            } while (currentRound < _gameConfiguration.MaximumRounds);
        }

        public abstract GameState GetGameState();

        public abstract void DoRound();

        public abstract void InvokePlayerAction(PlayerAction action);

        public virtual void Setup()
        {
        }

        protected void AddPlayer(Player player, PlayerState playerState)
        {
            _playerStates[player] = playerState;
        }
    }
}