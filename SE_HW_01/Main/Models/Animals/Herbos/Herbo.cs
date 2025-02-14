namespace Main.Models.Animals.Herbos
{
    public abstract class Herbo : Animal
    {
        protected byte kindnessLevel;
        protected byte requiredContactLevel;

        public byte KindnessLevel
        {
            get => kindnessLevel;
            set => kindnessLevel = Math.Min((byte)10, value);
        }

        public byte RequiredContactLevel
        {
            get => requiredContactLevel;
            set => requiredContactLevel = Math.Min((byte)10, value);
        }

        public Herbo(int number = 0, float food = 0, byte kindnessLevel = 0, byte requiredContactLevel = 5) : base(number, food)
        {
            KindnessLevel = kindnessLevel;
            RequiredContactLevel = requiredContactLevel;
        }

        public override bool IsContacting => RequiredContactLevel <= KindnessLevel;

        public override string ToString()
        {
            return "Травоядное животное";
        }
    }
}
