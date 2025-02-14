namespace Main.Models.Animals.Herbos
{
    public class Monkey : Herbo
    {
        public Monkey(int number = 0, float food = 0, byte kindnessLevel = 0) : base(number, food, kindnessLevel)
        { }

        public override string ToString()
        {
            return "Обезьяна";
        }
    }
}
