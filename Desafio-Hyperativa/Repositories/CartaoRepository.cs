using DesafioHyperativa.Context;
using DesafioHyperativa.CrossCutting.Contract;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Enum;
using DesafioHyperativa.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DesafioHyperativa.Repositories;

public class CartaoRepository : Repository<Cartao>, ICartaoRepository
{
    public CartaoRepository(ContextDb context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public IList<Cartao> GetCartoesJaInseridos(List<string> lst)
    {
        return this.DbSet.AsNoTracking().Where(x => lst.Contains(x.NumeroCartao)).ToList();
    }
}
