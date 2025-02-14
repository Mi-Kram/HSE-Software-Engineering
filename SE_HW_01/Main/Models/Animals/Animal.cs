using Main.Interfaces;

namespace Main.Models.Animals
{
    public abstract class Animal : IAlive, IInventory
    {
        protected float food;
        protected int number;

        public virtual float Food
        {
            get => food;
            set => food = value >= 0 ? value : throw new ArgumentException("food should be a positive number", nameof(Food));
        }

        public virtual int Number
        {
            get => number;
            set => number = value;
        }

        public abstract bool IsContacting { get; }

        public Animal() : this(0, 0)
        { }

        public Animal(int number, float food)
        {
            Number = number;
            Food = food;
        }

        public override string ToString()
        {
            return "Животное";
        }
    }
}
