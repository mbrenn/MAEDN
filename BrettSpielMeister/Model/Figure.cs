namespace BrettSpielMeister.Model
{
    public class Figure
    {
        public Figure()
        {
            FigureType = FigureType.Default;
        }

        public string? Name { get; set; }

        public FigureType? FigureType { get; set; }

        public Field? Field { get; set; }

        public override string ToString()
        {
            return Name ?? "No name";
        }
    }
}