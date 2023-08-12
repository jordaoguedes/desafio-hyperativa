using DesafioHyperativa.Entities;
using DesafioHyperativa.Service.Contract.Base.Contracts;

namespace DesafioHyperativa.Service.Contract;

public interface ICartaoLoteService : IService<CartaoLote>
{
    Task ProcessarSalvarArquivo(IFormFile file);
}
