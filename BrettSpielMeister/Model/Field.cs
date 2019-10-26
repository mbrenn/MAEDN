namespace BrettSpielMeister.Model
{
    public class Field
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string? Name { get; set; }
        public FieldType? FieldType { get; set; }

        public override string ToString()
        {
            return Name ?? "No name";
        }

        public Field()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Field
        /// </summary>
        /// <param name="x">Coordinate of X</param>
        /// <param name="y">Coordinate of Y</param>
        /// <param name="name">Name of the field</param>
        /// <param name="fieldType">Type of the field</param>
        public Field(int x, int y, string name, FieldType fieldType)
        {
            X = x;
            Y = y;
            Name = name;
            FieldType = fieldType;
        }

        /// <summary>
        /// Initializes a new instance of the Field
        /// </summary>
        /// <param name="x">Coordinate of X</param>
        /// <param name="y">Coordinate of Y</param>
        /// <param name="name">Name of the field</param>
        /// <param name="fieldType">Type of the field</param>
        public Field(int x, int y, FieldType fieldType)
        {
            X = x;
            Y = y;
            Name = string.Empty;
            FieldType = fieldType;
        }
    }
}
