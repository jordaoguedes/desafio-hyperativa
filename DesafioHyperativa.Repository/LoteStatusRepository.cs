using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Types;
using DesafioHyperativa.Repository.Context;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.CrossCutting.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DesafioHyperativa.Repository;

public class LoteStatusRepository : Repository<LoteStatus>, ILoteStatusRepository
{
    private readonly ILogger<LoteStatusRepository> _logger;
    public LoteStatusRepository(ContextDb context, IUnitOfWork unitOfWork, ILogger<LoteStatusRepository> logger) : base(context, unitOfWork)
    {
        _logger = logger;
    }

    public async Task<LoteStatus?> GetByGuid(string guid)
    {
        return await DbSet.Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }
}
