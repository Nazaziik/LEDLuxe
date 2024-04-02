namespace LEDLuxe.Core.Interfaces;

public interface IApplicationTransaction : IDisposable
{
    Task CommitAsync();

    Task RollbackAsync();
}