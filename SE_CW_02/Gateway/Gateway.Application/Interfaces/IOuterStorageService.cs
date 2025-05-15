using Gateway.Domain.Interfaces;

namespace Gateway.Application.Interfaces
{
    /// <summary>
    /// Интерфейс доступа к внешнему сервису для хранения работ.
    /// </summary>
    public interface IOuterStorageService : IStorageService;
}
