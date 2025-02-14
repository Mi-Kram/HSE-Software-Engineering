using Main.Interfaces;
using Main.Models.Animals;

namespace Main
{
    /// <summary>
    /// Клиника, которая с определённой вероятностью говорит, что животное здоровое.
    /// </summary>
    public class RandomVetClinic : IVetClinic
    {
        /// <summary>
        /// Весёлый ветеринар.
        /// </summary>
        private Random doctor = new Random();

        /// <summary>
        /// Вероятность положительного диагноза.
        /// </summary>
        private float healthyChance;

        /// <inheritdoc cref="healthyChance"/>
        public float HealthyChance
        {
            get => healthyChance;
            set => healthyChance = Math.Min(1f, Math.Max(0f, value));
        }

        public RandomVetClinic(float chance)
        {
            HealthyChance = chance;
        }


        public bool IsHealthy(Animal animal)
        {
            return doctor.NextDouble() <= healthyChance;
        }
    }
}
