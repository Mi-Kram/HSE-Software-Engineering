namespace FileAnalysisService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с работами.
    /// </summary>
    public interface IWorkRepository
    {
        /// <summary>
        /// Удаление данных, связанные с работой.
        /// </summary>
        /// <param name="workID">id работы.</param>
        Task DeleteAsync(int workID);
    }
}
