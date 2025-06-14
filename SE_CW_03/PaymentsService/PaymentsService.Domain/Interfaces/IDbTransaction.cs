namespace PaymentsService.Domain.Interfaces
{
    public interface IDbTransaction
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
