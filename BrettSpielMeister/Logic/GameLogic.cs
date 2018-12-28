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
        private readonly List<ActionRuleItem> _rules = new List<ActionRuleItem>();

        public TurnState TurnState { get; set; }

        public List<PlayerSet> PlayerSets { get;  } = new List<PlayerSet>();

        public abstract GameState GameState { get; }

        public Game Game { get; }

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

        /// <summary>
        /// Gets the player state of a certain player
        /// </summary>
        /// <param name="player">Player whose player state is queried</param>
        /// <returns>The state of the player</returns>
        public PlayerState GetPlayerState(Player player)
        {
            var foundPlayer = PlayerSets.FirstOrDefault(x => x.Player == player);
            if (foundPlayer != null)
            {
                UpdatePlayerState(foundPlayer);
                return foundPlayer.State;
            }

            return null;
        }

        /// <summary>
        /// Resets the game 
        /// </summary>
        public void Reset()
        {
            Game.Players.Clear();
            Game.Map.Clear();
        }

        public void Run()
        {
            Game.Map.Create();
            Setup();

            var currentRound = 0;

            do
            {
                DoRound();
                currentRound++;

                Console.WriteLine($"Round {currentRound} done.");
                Console.WriteLine($"---------------");


                Console.ReadKey();
                Console.WriteLine();

            } while (currentRound < _gameConfiguration.MaximumRounds);
        }

        public abstract PlayerState UpdatePlayerState(PlayerSet set);

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

        protected void AddPlayer(Player player, PlayerState playerState, PlayerBehavior behavior)
        {
            var set = new PlayerSet(player, playerState, behavior);
            PlayerSets.Add(set);

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

        /// <summary>
        /// Goes through the map and players and checks, if one of the figures
        /// is on the stated field
        /// </summary>
        /// <param name="field">Field to be queried</param>
        /// <returns>Found figure or null, if no figure is found</returns>
        public Figure IsFigureOnField(Field field)
        {
            return Game.Players
                .SelectMany(x => x.Figures)
                .FirstOrDefault(x => x.Field == field);
        }

        public PlayerSet GetPlayerSet(Player player)
        {
            return PlayerSets.FirstOrDefault(x => x.Player == player);
        }

        /// <summary>
        /// Checks, if the player has a certain figure on the field
        /// </summary>
        /// <param name="player">Player to be evaluated</param>
        /// <param name="field">Field to be evaluated</param>
        /// <returns>true, if the given player has a figure on the field</returns>
        public static bool HasFigureOnField(PlayerSet player, Field field)
        {
            return player.Player.Figures.Any(
                x => x.Field == field);
        }
    }
}