using BrettSpielMeister.Model;

namespace BrettSpielMeister.Actions
{
    public class MoveFigureAction : PlayerAction
    {
        public MoveFigureAction(Figure figure, Field targetField)
        {
            Figure = figure;
            TargetField = targetField;
        }

        public Figure Figure { get; set; }

        public Field TargetField { get; set; }
    }
}