using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Context;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.CrossCutting.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DesafioHyperativa.Repository;

public class LoteRepository : Repository<Lote>, ILoteRepository
{
    private readonly ILogger<LoteRepository> _logger;
    public LoteRepository(ContextDb context, IUnitOfWork unitOfWork, ILogger<LoteRepository> logger) : base(context, unitOfWork)
    {
        _logger = logger;
    }

    public bool VerificarLoteExiste(string nome)
    {
        return this.DbSet.AsNoTracking().Any(x => x.Nome == nome);
    }
}
