using Main.Models.Animals;

namespace Main.Interfaces
{
    /// <summary>
    /// Интерфейс ветеринарной клиники.
    /// </summary>
    public interface IVetClinic
    {
        /// <summary>
        /// Проверка здоровья животного.
        /// </summary>
        /// <param name="animal">Животное для проверки.</param>
        /// <returns>True, если животное здоровое, иначе - False.</returns>
        bool IsHealthy(Animal animal);
    }
}
