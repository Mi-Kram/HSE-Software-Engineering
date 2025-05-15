using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Interfaces;
using FileStoringService.Domain.Interfaces.Scripts;
using FileStoringService.Infrastructure.Persistence.Scripts;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Infrastructure.Persistence
{
    /// <summary>
    /// Репозиторий работ.
    /// </summary>
    public class WorkRepository(WorkDbContext context) : IWorkRepository
    {
        private readonly WorkDbContext context = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Получить коллекцию всех работ.
        /// </summary>
        /// <returns>Коллекция всех работ.</returns>
        public async Task<IEnumerable<WorkInfo>> GetAllAsync()
        {
            return await context.Works.ToListAsync();
        }

        /// <summary>
        /// Получить коллекцию всех работ одного владельца.
        /// </summary>
        /// <param name="hash">хэш работы.</param>
        /// <returns>Коллекция всех работ одного владельца.</returns>
        public async Task<IEnumerable<WorkInfo>> GetAllByHashAsync(string hash)
        {
            return await context.Works.Where(x => x.Hash == hash).ToListAsync();
        }

        /// <summary>
        /// Получить коллекцию всех работ одного владельца.
        /// </summary>
        /// <param name="userID">id владельца.</param>
        /// <returns>Коллекция всех работ одного владельца.</returns>
        public async Task<IEnumerable<WorkInfo>> GetAllByUserIDAsync(int userID)
        {
            return await context.Works.Where(x => x.UserID == userID).ToListAsync();
        }

        /// <summary>
        /// Получить работу по id.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <returns></returns>
        public async Task<WorkInfo?> GetAsync(int workID)
        {
            return await context.Works.FindAsync(workID);
        }

        /// <summary>
        /// Получение сценария на добавление работы.
        /// </summary>
        /// <returns>Сценарий на добавление работы.</returns>
        public Task<IAddWorkScript> GetAddScriptAsync()
        {
            return Task.FromResult<IAddWorkScript>(new AddWorkScript(context));
        }

        /// <summary>
        /// Получение сценария на удаление работы.
        /// </summary>
        /// <returns>Сценарий на удаление работы.</returns>
        public Task<IRemoveWorkScript> GetRemoveScriptAsync()
        {
            return Task.FromResult<IRemoveWorkScript>(new RemoveWorkScript(context));
        }
    }
}
