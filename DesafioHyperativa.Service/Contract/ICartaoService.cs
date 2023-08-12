using DesafioHyperativa.Entities;
using DesafioHyperativa.Service.Contract.Base.Contracts;

namespace DesafioHyperativa.Service.Contract;

public interface ICartaoService : IService<Cartao>
{
    Task InserirCartao(string numeroCartao);
    Task InserirCartaoCriptografado(string numeroCartaoCriptografado);
    Task<int> GetIdByNumeroCartao(string numeroCartao);
    Task<int> GetIdByNumeroCartaoCriptografado(string numeroCartaoCriptografado);
}
