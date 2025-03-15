using System.Numerics;

namespace HseBankLibrary.Storage.AutoIncrement
{
    /// <summary>
    /// Интерфейс для самоувеличивающегося ID.
    /// </summary>
    public interface IAutoIncrement<T>
    {
        /// <summary>
        /// Получить следующий ID.
        /// </summary>
        /// <returns>Следующий ID.</returns>
        T GetNext();
    }
}
