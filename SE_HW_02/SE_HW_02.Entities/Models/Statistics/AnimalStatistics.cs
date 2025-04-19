namespace SE_HW_02.Entities.Models.Statistics
{
    /// <summary>
    /// Информация статистики животных.
    /// </summary>
    public class AnimalsStatistics
    {
        /// <summary>
        /// Общее количество.
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// Максимальная вместимость.
        /// </summary>
        public int TotalCapacity { get; set; }

        /// <summary>
        /// Количество здоровых животных.
        /// </summary>
        public int HealthyAmount { get; set; }

        /// <summary>
        /// Количество животных мужского пола.
        /// </summary>
        public int MaleAmount { get; set; }

        /// <summary>
        /// Словарь [тип животного : количество животных такого типа].
        /// </summary>
        public Dictionary<string, int> TypeAmount { get; } = new();
    }
}
