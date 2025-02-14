namespace Main.Models.Animals.Predators
{
    public class Predator : Animal
    {
        public Predator(int number = 0, float food = 0) : base(number, food)
        { }

        public override bool IsContacting => false;

        public override string ToString()
        {
            return "Хищное животное";
        }
    }
}
