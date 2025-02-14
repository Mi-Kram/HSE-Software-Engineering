namespace Main.Models.Things
{
    public class Table : Thing
    {
        public Table(int number = 0) : base(number)
        { }

        public override string ToString()
        {
            return "Стол";
        }
    }
}
