namespace BrettSpielMeister.Model
{
    public class FieldType
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the character being used for the outputting to the console
        /// </summary>
        public char Character { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}