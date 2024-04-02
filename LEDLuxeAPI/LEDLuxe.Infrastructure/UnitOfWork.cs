using LEDLuxe.Core.Interfaces;

namespace LEDLuxe.Infrastructure;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IApplicationTransaction> BeginTransactionAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        return new ApplicationTransaction(transaction);
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}