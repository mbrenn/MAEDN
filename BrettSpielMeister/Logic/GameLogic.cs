using BrettSpielMeister.Model;
using BrettSpielMeister.States;

namespace BrettSpielMeister.Logic
{
    public class GameLogic
    {
        public GameLogic(Game game)
        {
            Game = game;
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
        }

        public virtual GameState EvaluateGameState()
        {
            return new GameState();
        }

        public virtual void Setup()
        {
        }
    }
}