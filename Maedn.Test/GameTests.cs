using System;
using System.Linq;
using MAEDN.Rules;
using Xunit;

namespace Maedn.Test
{
    public class GameTests
    {
        [Fact]
        public void CheckGameLogic()
        {
            var game = GetMaednGame();

            Assert.Equal(2, game.PlayerSets.Count);
            Assert.Equal(4, game.PlayerSets[0].Player.Figures.Count);
            Assert.Equal(4, game.PlayerSets[1].Player.Figures.Count);
        }

        [Fact]
        public void CheckHomeEvaluation()
        {
            var game = GetMaednGame();

            var firstFigure = game.PlayerSets[0].Player.Figures[0];
            var state = (MaednPlayerState) game.UpdatePlayerState(game.PlayerSets[0]);
            Assert.True(MaednLogic.IsFigureInHome(game.PlayerSets[0], firstFigure));

            firstFigure.Field = game.Map.MovingFields.ElementAt(3);
            Assert.False(MaednLogic.IsFigureInHome(game.PlayerSets[0], firstFigure));

            firstFigure.Field = game.Map.RedGoalFields.ElementAt(3);
            Assert.False(MaednLogic.IsFigureInHome(game.PlayerSets[0], firstFigure));

            firstFigure.Field = game.Map.RedHomeFields.ElementAt(3);
            Assert.True(MaednLogic.IsFigureInHome(game.PlayerSets[0], firstFigure));

            firstFigure.Field = game.Map.BlueHomeFields.ElementAt(3);
            Assert.False(MaednLogic.IsFigureInHome(game.PlayerSets[0], firstFigure));
        }

        [Fact]
        public void CheckStartEvaluation()
        {
            var game = GetMaednGame();

            var firstFigure = game.PlayerSets[0].Player.Figures[0];
            var state = (MaednPlayerState)game.UpdatePlayerState(game.PlayerSets[0]);
            Assert.False(MaednLogic.IsFigureInStart(game.PlayerSets[0], firstFigure));

            firstFigure.Field = state.StartField;
            Assert.True(MaednLogic.IsFigureInStart(game.PlayerSets[0],firstFigure));

            firstFigure.Field = ((MaednPlayerState) game.PlayerSets[1].State).StartField;
            Assert.False(MaednLogic.IsFigureInStart(game.PlayerSets[0], firstFigure));
        }

        [Fact]
        public void CheckThreeDice()
        {
            var game = GetMaednGame();
            var firstPlayerSet = game.PlayerSets[0];
            var firstPlayerState = (MaednPlayerState) game.PlayerSets[0].State;
            var firstFigure = game.PlayerSets[0].Player.Figures[0];
            var secondFigure = game.PlayerSets[0].Player.Figures[1];
            var thirdFigure = game.PlayerSets[0].Player.Figures[2];
            var fourthFigure = game.PlayerSets[0].Player.Figures[2];

            Assert.True(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));

            firstFigure.Field = game.Map.MovingFields.ElementAt(2);
            Assert.False(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));

            firstFigure.Field = firstPlayerState.GoalFields.ElementAt(0);
            Assert.False(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));

            firstFigure.Field = firstPlayerState.GoalFields.ElementAt(1);
            Assert.False(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));

            firstFigure.Field = firstPlayerState.GoalFields.ElementAt(3);
            Assert.True(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));

            secondFigure.Field = game.Map.MovingFields.ElementAt(4);
            Assert.False(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));

            secondFigure.Field = firstPlayerState.GoalFields.ElementAt(2);
            Assert.True(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));

            secondFigure.Field = firstPlayerState.GoalFields.ElementAt(1);
            Assert.False(MaednLogic.AreFiguresPressedInGoalOrHome(firstPlayerSet));
        }



        private static MaednLogic GetMaednGame()
        {
            var game = new MaednLogic(new MaednConfiguration());
            game.Map.Create();
            game.Setup();
            return game;
        }
    }
}
