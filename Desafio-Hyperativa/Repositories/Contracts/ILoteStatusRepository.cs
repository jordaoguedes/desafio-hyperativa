using DesafioHyperativa.Entities;

namespace DesafioHyperativa.Repositories.Contracts;

public interface ILoteStatusRepository : IRepository<LoteStatus>
{
    Task<IList<LoteStatus>> GetAll();
    Task<LoteStatus?> GetByGuid(string guid);
}
