using DesafioHyperativa.Repository.Context;
using DesafioHyperativa.Repository.CrossCutting.Contract;

namespace DesafioHyperativa.Repository.CrossCutting;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContextDb _context;
    private string _transactionGuid = string.Empty;

    public UnitOfWork(ContextDb context)
    {
        _context = context;
    }


    public void BeginTransaction(string transactionGuid)
    {
        if (string.IsNullOrEmpty(_transactionGuid))
        {
            _transactionGuid = transactionGuid;
        }

    }

    public async Task CommitAsync(string transactionGuid)
    {
        if (transactionGuid == _transactionGuid)
        {
            await _context.SaveChangesAsync();
        }
    }

    public Task RollBackAsync(string transactionGuid)
    {
        _context.ChangeTracker.Clear();
        return Task.CompletedTask;
    }
    public void Dispose()
    {
        if (_context != null)
        {
            _context.Dispose();
        }
    }

}
