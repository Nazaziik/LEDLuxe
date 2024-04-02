namespace LEDLuxe.Core.Interfaces;

public interface IUnitOfWork
{
    Task<IApplicationTransaction> BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}