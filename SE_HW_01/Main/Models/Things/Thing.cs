using Main.Interfaces;

namespace Main.Models.Things
{
    public abstract class Thing : IInventory
    {
        protected int number;

        public virtual int Number
        {
            get => number;
            set => number = value;
        }

        public Thing(int number = 0)
        {
            Number = number;
        }

        public override string ToString()
        {
            return "Предмет";
        }
    }
}
