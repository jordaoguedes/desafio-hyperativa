﻿using DesafioHyperativa.Entities;
using DesafioHyperativa.Service.Contract.Base.Contracts;

namespace DesafioHyperativa.Service.Contract;

public interface ILoteStatusService : IService<LoteStatus>
{
    Task<IList<LoteStatus>> GetAll();
    Task<string> ProcessarArquivo(IFormFile file);
    Task<LoteStatus?> VerificarGuid(string guid);
}
