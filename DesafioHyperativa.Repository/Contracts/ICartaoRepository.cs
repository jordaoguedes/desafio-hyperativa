using DesafioHyperativa.Entities;

namespace DesafioHyperativa.Repository.Contracts;

public interface ICartaoRepository : IRepository<Cartao>
{
    Task<Cartao?> GetCartaoByNumero(string numeroCartao);
    Task<IList<Cartao>> GetCartoesJaInseridos(List<string> lst);
    Task<int> GetIdByNumeroAsync(string numeroCripografado);
}
