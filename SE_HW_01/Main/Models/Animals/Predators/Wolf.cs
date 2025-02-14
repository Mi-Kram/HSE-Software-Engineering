namespace Main.Models.Animals.Predators
{
    public class Wolf : Predator
    {
        public Wolf(int number = 0, float food = 0) : base(number, food)
        { }

        public override string ToString()
        {
            return "Волк";
        }
    }
}
