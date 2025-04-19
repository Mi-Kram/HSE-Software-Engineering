using SE_HW_02.Entities.Models;

namespace SE_HW_02.Web.DTO
{
    /// <summary>
    /// DTO для получения информации о вольере.
    /// </summary>
    public class EnclosureDTO
    {
        /// <summary>
        /// Тип вольера.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Размер вольера.
        /// </summary>
        public Dimensions Size { get; set; }

        /// <summary>
        /// Вместимость животных.
        /// </summary>
        public int AnimalsCapacity { get; set; }
    }
}
