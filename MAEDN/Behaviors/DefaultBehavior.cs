﻿using System;
using BrettSpielMeister.Actions;
using BrettSpielMeister.Logic;
using BrettSpielMeister.Model;
using BrettSpielMeister.States.Turns;
using BurnSystems.Logging;
using MAEDN.Rules;

namespace MAEDN.Behaviors
{
    public class DefaultBehavior : PlayerBehavior
    {
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(DefaultBehavior));
        private readonly MaednLogic _gameLogic;
        private readonly Player _player;

        public DefaultBehavior(MaednLogic gameLogic, Player player)
        {
            _player = player;
            _gameLogic = gameLogic;
        }

        public override PlayerAction PerformTurn(GameLogic gameLogic)
        {
            if (gameLogic.TurnState is TurnDiceState)
            {
                return new DiceAction();
            }

            if (gameLogic.TurnState is TurnMoveFigureState)
            {
                var allowedTurns = _gameLogic.GetAllowedTurns();
                if (allowedTurns.Count == 0)
                {
                    return new DoNothingAction();
                }

                var result = new MoveFigureAction(
                    allowedTurns[0].Figure, 
                    allowedTurns[0].TargetField);

                return result;
            }

            ClassLogger.Error("Player cannot handle current situation");
            throw new InvalidOperationException("Player cannot handle current situation");
        }
    }
}
