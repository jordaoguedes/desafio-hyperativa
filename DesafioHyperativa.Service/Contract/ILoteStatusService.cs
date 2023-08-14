using DesafioHyperativa.Entities;
using DesafioHyperativa.Service.Contract.Base.Contracts;
using Microsoft.AspNetCore.Http;

namespace DesafioHyperativa.Service.Contract;

public interface ILoteStatusService : IService<LoteStatus>
{
    Task<string> ProcessarArquivo(IFormFile file);
    Task<LoteStatus?> VerificarGuid(string guid);
}
