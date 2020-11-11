using System.Linq;
using MAEDN.Rules;
using Xunit;

namespace Maedn.Test
{
    public class MapTests
    {
        [Fact]
        public void TestMapForward()
        {
            var game = GameTests.GetMaednGame();
            var playerSet = game.PlayerSets[0];     
            var figure = playerSet.Player.Figures[0];

            figure.Field = game.Map.MovingFields.ElementAt(10);

            var foundField = game.GetNextFieldForPlayer(playerSet, figure.Field, 10);
            Assert.Equal(game.Map.MovingFields.ElementAt(20), foundField);
        }

        [Fact]
        public void TestCompleteRound()
        {
            var game = GameTests.GetMaednGame();
            var playerSet = game.PlayerSets[0];
            var figure = playerSet.Player.Figures[0];

            figure.Field = game.Map.MovingFields.ElementAt(10);
            var startFigure = figure.Field;

            var foundField = game.GetNextFieldForPlayer(playerSet, figure.Field, 40);
            Assert.Equal(startFigure, foundField);
        }

        [Fact]
        public void TestGettingToGoal()
        {
            var game = GameTests.GetMaednGame();
            var playerSet = game.PlayerSets[0];
            var figure = playerSet.Player.Figures[0];

            figure.Field = playerSet.GetMaednPlayerState().StartField;
            figure.Field = game.GetNextFieldForPlayer(playerSet, figure.Field, 35);

            Assert.Equal(game.Map.MovingFields.ElementAt(35), figure.Field);
            Assert.True(figure.Field != null);

            figure.Field = game.GetNextFieldForPlayer(playerSet, figure.Field, 4);

            Assert.Equal(game.Map.MovingFields.ElementAt(39), figure.Field);
            Assert.True(figure.Field != null);

            var field = game.GetNextFieldForPlayer(playerSet, figure.Field, 1);
            Assert.Equal(
                playerSet.GetMaednPlayerState().GoalFields.ElementAt(0),
                field);

            field = game.GetNextFieldForPlayer(playerSet, figure.Field, 3);
            Assert.Equal(
                playerSet.GetMaednPlayerState().GoalFields.ElementAt(2),
                field);

            field = game.GetNextFieldForPlayer(playerSet, figure.Field, 4);
            Assert.Equal(
                playerSet.GetMaednPlayerState().GoalFields.ElementAt(3),
                field);

            field = game.GetNextFieldForPlayer(playerSet, figure.Field, 5);
            Assert.Null(
                field);
        }
    }
}