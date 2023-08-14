using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Context;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.CrossCutting.Contract;
using Microsoft.EntityFrameworkCore;

namespace DesafioHyperativa.Repository;

public class LoteRepository : Repository<Lote>, ILoteRepository
{
    public LoteRepository(ContextDb context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public bool VerificarLoteExiste(string nome)
    {
        return this.DbSet.AsNoTracking().Any(x => x.Nome == nome);
    }
}
