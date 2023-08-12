using DesafioHyperativa.Entities;
using DesafioHyperativa.Service.Contract.Base.Contracts;
using Microsoft.AspNetCore.Http;

namespace DesafioHyperativa.Service.Contract;

public interface ICartaoLoteService : IService<CartaoLote>
{
    Task ProcessarSalvarArquivo(IFormFile file);
}
