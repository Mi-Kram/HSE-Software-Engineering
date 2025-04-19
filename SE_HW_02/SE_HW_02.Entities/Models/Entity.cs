namespace SE_HW_02.Entities.Models
{
    /// <summary>
    /// Базовый класс сущности.
    /// </summary>
    public  abstract class Entity
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public abstract int ID { get; set; }

        /// <summary>
        /// Создание копии объекта.
        /// </summary>
        /// <returns>Копия объекта.</returns>
        public abstract Entity Clone();
    }
}
