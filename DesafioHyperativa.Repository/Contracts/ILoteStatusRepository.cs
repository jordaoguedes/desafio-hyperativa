using DesafioHyperativa.Entities;

namespace DesafioHyperativa.Repository.Contracts;

public interface ILoteStatusRepository : IRepository<LoteStatus>
{
    Task<LoteStatus?> GetByGuid(string guid);
}
