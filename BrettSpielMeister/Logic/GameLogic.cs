using System;
using System.Collections.Generic;
using System.Linq;
using BrettSpielMeister.Actions;
using BrettSpielMeister.Model;
using BrettSpielMeister.Output;
using BrettSpielMeister.States;

namespace BrettSpielMeister.Logic
{
    public abstract class GameLogic
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly Dictionary<Player, PlayerState> _playerStates = new Dictionary<Player, PlayerState>();
        private readonly List<ActionRuleItem> _rules = new List<ActionRuleItem>();

        public GameLogic(GameConfiguration gameConfiguration, Game game)
        {
            _gameConfiguration = gameConfiguration;
            Game = game;
        }

        /// <summary>
        /// Adds an filter rule item that will been called, if the user triggers an filter
        /// InvokePlayerAction. 
        /// </summary>
        /// <param name="filter">The predicate determining whether the rule is valued</param>
        /// <param name="rule">Rule being executed, when the filter is matching</param>
        public void AddRule(Predicate<PlayerAction> filter, PlayerActionHandler rule)
        {
            _rules.Add(new ActionRuleItem(filter, rule));
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

            var currentRound = 0;

            do
            {
                var gameState = GetGameState();

                DoRound();
                currentRound++;

                Console.WriteLine($"Round {currentRound} done.");
                new MapToConsole().Write(Game);

                Console.ReadKey();
                Console.WriteLine();

            } while (currentRound < _gameConfiguration.MaximumRounds);
        }

        public abstract GameState GetGameState();

        public abstract void DoRound();

        public abstract void Setup();

        /// <summary>
        /// This methods will be called be the player behaviors and selects the playeraction
        /// being executed
        /// </summary>
        /// <param name="player">Player calling this method</param>
        /// <param name="action">Action to be executed</param>
        public void InvokePlayerAction(Player player, PlayerAction action)
        {
            var rule = _rules.FirstOrDefault(x => x.Filter(action));
            if (rule != null)
            {
                rule.ActionRule.Invoke(this, player, action);
            }
            else
            {
                throw new InvalidOperationException($"Rule is not known: {action}");
            }
        }

        protected void AddPlayer(Player player, PlayerState playerState)
        {
            _playerStates[player] = playerState;
            Game.Players.Add(player);
        }

        public class ActionRuleItem
        {
            public ActionRuleItem(Predicate<PlayerAction> filter, PlayerActionHandler actionRule)
            {
                Filter = filter;
                ActionRule = actionRule;
            }

            public Predicate<PlayerAction> Filter { get; }
            public PlayerActionHandler ActionRule { get; }
        }
    }
}