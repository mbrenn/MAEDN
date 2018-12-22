using System;
using System.Collections.Generic;
using System.Text;
using BrettSpielMeister.Actions;
using BrettSpielMeister.Logic;
using BrettSpielMeister.Model;
using BrettSpielMeister.States;

namespace MAEDN.Behaviors
{
    class DefaultBehavior : PlayerBehavior
    {
        private readonly Player _player;

        public DefaultBehavior(Player player)
        {
            _player = player;
        }

        public override PlayerAction PerformTurm(GameLogic gameLogic)
        {
            if (gameLogic.TurnState is TurnDiceState)
            {
                return new DiceAction();
            }

            return null;
        }
    }
}
