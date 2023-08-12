using DesafioHyperativa.Context;
using DesafioHyperativa.CrossCutting.Contract;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Enum;
using DesafioHyperativa.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DesafioHyperativa.Repositories;

public class LoteStatusRepository : Repository<LoteStatus>, ILoteStatusRepository
{
    public LoteStatusRepository(ContextDb context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public async Task<IList<LoteStatus>> GetAll()
    {
        return await this.DbSet.Where(x => x.Status == StatusType.ACCEPTED).ToListAsync();
    }

    public async Task<LoteStatus?> GetByGuid(string guid)
    {
        return await this.DbSet.Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }
}
