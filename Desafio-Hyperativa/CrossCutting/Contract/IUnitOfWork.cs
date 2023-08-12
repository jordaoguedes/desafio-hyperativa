namespace DesafioHyperativa.CrossCutting.Contract;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction(string transactionGuid);
    Task CommitAsync(string transactionGuid);
    Task RollBackAsync(string transactionGuid);
}
