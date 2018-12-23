using BrettSpielMeister.Model;

namespace BrettSpielMeister.Actions
{
    public class AllowedTurn
    {
        public AllowedTurn(Figure figure, Field targetField)
        {
            Figure = figure;
            TargetField = targetField;
        }

        /// <summary>
        /// Gets the figure that is allowed to move
        /// </summary>
        public Figure Figure { get; }

        /// <summary>
        /// Gets the target field for movement
        /// </summary>
        public Field TargetField { get; }
    }
}