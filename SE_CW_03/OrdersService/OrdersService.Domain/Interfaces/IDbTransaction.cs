namespace OrdersService.Domain.Interfaces
{
    public interface IDbTransaction
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
