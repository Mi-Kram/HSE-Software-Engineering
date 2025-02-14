namespace Main.Models.Things
{
    public class Computer : Thing
    {
        public Computer(int number = 0) : base(number)
        { }

        public override string ToString()
        {
            return "Компьютер";
        }
    }
}
