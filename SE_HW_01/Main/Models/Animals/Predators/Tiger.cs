namespace Main.Models.Animals.Predators
{
    public class Tiger : Predator
    {
        public Tiger(int number = 0, float food = 0) : base(number, food)
        { }

        public override string ToString()
        {
            return "Тигр";
        }
    }
}
