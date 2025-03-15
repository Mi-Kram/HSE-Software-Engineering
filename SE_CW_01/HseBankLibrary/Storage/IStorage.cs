using HseBankLibrary.Models.Domain;

namespace HseBankLibrary.Storage
{
    /// <summary>
    /// Интерфес репозитория.
    /// </summary>
    /// <typeparam name="T_ID">Тип идентификатора сущности.</typeparam>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <typeparam name="T_DTO">Тип DTO сущности.</typeparam>
    public interface IStorage<T_ID, T, T_DTO> where T : IEntity<T_ID, T_DTO> where T_ID : notnull
    {
        /// <summary>
        /// Получить объект по id.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект с указанным id, иначе null.</returns>
        T? Get(T_ID id);

        /// <summary>
        /// Получить все объекты.
        /// </summary>
        /// <returns>Перечисление всех объектов.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Добавление нового объекта.
        /// </summary>
        /// <param name="item">Новый объект.</param>
        /// <returns>ID нового объекта.</returns>
        T_ID Add(T item);

        /// <summary>
        /// Обновление объекта.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="item">Обновлённый объект.</param>
        /// <returns></returns>
        bool Update(T_ID id, T item);

        /// <summary>
        /// Удаление объекта.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>True, если удаление произошло, иначе - False.</returns>
        bool Delete(T_ID id);

        /// <summary>
        /// Проверка доступности репозитория.
        /// </summary>
        /// <returns>True, если репозиторий доступен, иначе - False.</returns>
        bool Ping();

        /// <summary>
        /// Пересаписать репозиторий другим репозиторием.
        /// </summary>
        /// <param name="storage">Другое репозиторий.</param>
        void Rewrite(IStorage<T_ID, T, T_DTO> storage);
    }
}
