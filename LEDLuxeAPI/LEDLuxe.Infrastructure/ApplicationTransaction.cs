using LEDLuxe.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace LEDLuxe.Infrastructure;

public class ApplicationTransaction(IDbContextTransaction transaction) : IApplicationTransaction
{
    private readonly IDbContextTransaction _transaction = transaction;

    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }
}