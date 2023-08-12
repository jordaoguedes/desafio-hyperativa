using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Types;
using DesafioHyperativa.Repository.Context;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.CrossCutting.Contract;
using Microsoft.EntityFrameworkCore;

namespace DesafioHyperativa.Repository;

public class LoteStatusRepository : Repository<LoteStatus>, ILoteStatusRepository
{
    public LoteStatusRepository(ContextDb context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public async Task<IList<LoteStatus>> GetAll()
    {
        return await DbSet.Where(x => x.Status == StatusType.ACCEPTED).ToListAsync();
    }

    public async Task<LoteStatus?> GetByGuid(string guid)
    {
        return await DbSet.Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }
}
