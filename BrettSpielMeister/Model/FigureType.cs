namespace BrettSpielMeister.Model
{
    public class FigureType
    {
        public string? Name { get; set; }

        public char Character { get; set; }

        public static FigureType Default => new FigureType
        {
            Name = "Default",
            Character = 'f'
        };

        public override string ToString()
        {
            return Name ?? "No name";
        }
    }
}