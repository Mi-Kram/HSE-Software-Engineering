namespace FileAnalysisService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с работами.
    /// </summary>
    public interface IWorkService
    {
        /// <summary>
        /// Удаление данных, связанные с работой.
        /// </summary>
        /// <param name="workID">id работы.</param>
        Task DeleteAsync(int workID);
    }
}
