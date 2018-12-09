namespace BrettSpielMeister.Model
{
    public class Figure
    {
        public string Name { get; set; }

        public Field Field { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}