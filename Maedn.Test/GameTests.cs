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

        [Fact]
        public void CheckAllowedMovementsIfNotDiced()
        {
            var game = GetMaednGame();
            Assert.False(game.Dice.DiceState.IsDiced);

            game.GameState.CurrentPlayer = game.Game.Players[0];

            var allowedTurns = game.GetAllowedTurns();
            Assert.Empty(allowedTurns);
        }

        [Fact]
        public void CheckAllowedMovementsWhenEverybodyIsAtHomeIfDiced()
        {
            var game = GetMaednGame();
            Assert.False(game.Dice.DiceState.IsDiced);
            game.Dice.DiceState.IsDiced = true;
            game.Dice.DiceState.CurrentDiceValue = 5;

            game.GameState.CurrentPlayer = game.Game.Players[0];

            var allowedTurns = game.GetAllowedTurns();
            Assert.Empty(allowedTurns);

            game.Dice.DiceState.IsDiced = true;
            game.Dice.DiceState.CurrentDiceValue = 6;

            allowedTurns = game.GetAllowedTurns();
            Assert.Equal(4, allowedTurns.Count);

            var firstTurn = allowedTurns.ElementAt(0);
            Assert.Equal(game.Map.RedStartField, firstTurn.TargetField);
        }


        [Fact]
        public void CheckAllowedMovementsWhenAtStartingField()
        {
            var game = GetMaednGame();
            Assert.False(game.Dice.DiceState.IsDiced);

            // One figure on starting field
            var currentPlayer = game.GameState.CurrentPlayer = game.Game.Players[0];
            currentPlayer.Figures[0].Field = game.PlayerSets[0].GetMaednPlayerState().StartField;
            
            game.Dice.DiceState.IsDiced = true;
            game.Dice.DiceState.CurrentDiceValue = 5;

            var allowedTurns = game.GetAllowedTurns();
            Assert.Single(allowedTurns);
            Assert.Equal(game.Map.MovingFields.ElementAt(5), allowedTurns[0].TargetField);

            currentPlayer.Figures[1].Field = game.Map.MovingFields.ElementAt(10);
            allowedTurns = game.GetAllowedTurns();
            Assert.Single(allowedTurns);
            Assert.Equal(game.Map.MovingFields.ElementAt(5), allowedTurns[0].TargetField);

            
            currentPlayer.Figures[1].Field = game.Map.MovingFields.ElementAt(5);
            allowedTurns = game.GetAllowedTurns();
            Assert.Single(allowedTurns);
            Assert.Equal(game.Map.MovingFields.ElementAt(10), allowedTurns[0].TargetField);
        }

        public static MaednLogic GetMaednGame()
        {
            var game = new MaednLogic(new MaednConfiguration());
            game.Map.Create();
            game.Setup();
            return game;
        }
    }
}