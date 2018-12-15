using System;
using System.Collections.Generic;
using System.Text;
using BrettSpielMeister.Logic;
using BrettSpielMeister.Model;

namespace MAEDN.Behaviors
{
    class DefaultBehavior : PlayerBehavior
    {
        private readonly Player _player;

        public DefaultBehavior(Player player)
        {
            _player = player;
        }

        public override void PerformTurm(GameLogic gameLogic)
        {
            throw new NotImplementedException();
        }
    }
}
