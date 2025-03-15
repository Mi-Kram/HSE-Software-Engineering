namespace HseBankLibrary.Models.Domain
{
    /// <summary>
    /// Интерфейс сущности.
    /// </summary>
    public interface IEntity<T_ID> where T_ID : notnull
    {
        /// <summary>
        /// Уникальный идентификатор сущности.
        /// </summary>
        T_ID ID { get; set; }
    }

    /// <summary>
    /// Расширенный интерфейс сущности: хранение информации о DTO сущности.
    /// </summary>
    public interface IEntity<T_ID, DTO> : IEntity<T_ID> where T_ID : notnull
    {
        /// <summary>
        /// Получение DTO модели.
        /// </summary>
        /// <returns>DTO модель.</returns>
        DTO ToDTO();
    }
}
