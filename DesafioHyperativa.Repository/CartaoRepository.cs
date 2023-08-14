using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Context;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.CrossCutting.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DesafioHyperativa.Repository;

public class CartaoRepository : Repository<Cartao>, ICartaoRepository
{
    private readonly ILogger<CartaoRepository> _logger;
    public CartaoRepository(ContextDb context, IUnitOfWork unitOfWork, ILogger<CartaoRepository> logger) : base(context, unitOfWork)
    {
        _logger = logger;
    }

    public async Task<Cartao?> GetCartaoByNumero(string numeroCartao)
    {
        return await DbSet.Where(x => x.NumeroCartao == numeroCartao).FirstOrDefaultAsync();
    }

    public async Task<IList<Cartao>> GetCartoesJaInseridos(List<string> lst)
    {
        return await DbSet.AsNoTracking().Where(x => lst.Contains(x.NumeroCartao)).ToListAsync();
    }

    public Task<int> GetIdByNumeroAsync(string numeroCripografado)
    {
        return DbSet.AsNoTracking().Where(x => x.NumeroCartao == numeroCripografado).Select(x => x.Id).FirstOrDefaultAsync();
    }
}
