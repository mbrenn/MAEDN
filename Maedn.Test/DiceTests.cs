using System;
using BrettSpielMeister.Logic;
using BrettSpielMeister.States;
using Xunit;
using Xunit.Abstractions;

namespace Maedn.Test
{
    public class DiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestDice()
        {
            var dices = new int[6];
            var dice = new Dice(new DiceState());
            var iterations = 100000;

            for (var n = 0; n < iterations; n++)
            {
                dice.ThrowDice();
                dices[dice.DiceState.CurrentDiceValue - 1]++;
                dice.PickupDice();
            }

            for (var n = 0; n < dices.Length; n++)
            {
                var count = dices[n];
                Assert.True(count > (iterations / 10));
                _testOutputHelper.WriteLine($"{n+1}: {count}");
            }
        }
    }
}