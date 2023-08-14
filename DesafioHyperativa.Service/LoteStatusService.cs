using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Types;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.Util;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Contract.Base;
using DesafioHyperativa.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioHyperativa.Service;

public class LoteStatusService : Service<LoteStatus>, ILoteStatusService
{
    private readonly ILoteStatusRepository _repositoryLoteStatus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public LoteStatusService(ILoteStatusRepository repositoryLoteStatus, IServiceScopeFactory serviceScopeFactory) : base(repositoryLoteStatus)
    {
        _repositoryLoteStatus = repositoryLoteStatus;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<string> ProcessarArquivo(IFormFile file)
    {
        var loteStatus = new LoteStatus
        {
            Guid = Guid.NewGuid().ToString(),
            Status = StatusType.ACCEPTED,
            Erro = "",
            File = Extension.EncryptFileToBytes(file)
        };
        await this.Repository.SaveAsync(loteStatus);

        ProcessarInBackGround(loteStatus.Id);

        return loteStatus.Guid.ToString();
    }

    public async Task<LoteStatus?> VerificarGuid(string guid)
    {
        return await _repositoryLoteStatus.GetByGuid(guid);
    }

    private async Task ProcessarInBackGround(int id)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var serviceLote = scope.ServiceProvider.GetRequiredService<ICartaoLoteService>();
            var serviceLoteStatus = scope.ServiceProvider.GetRequiredService<ILoteStatusService>();

            var entity = await serviceLoteStatus.GetAsync(id);
            entity.Status = StatusType.IN_PROGRESS;
            await serviceLoteStatus.SaveAsync(entity);

            try
            {
                await serviceLote.ProcessarSalvarArquivo(Extension.DecryptBytesToFormFile(entity.File));
                entity.Status = StatusType.FINISHED;
                await serviceLoteStatus.SaveAsync(entity);
            }
            catch (BadRequestException ex)
            {
                entity.Erro = ex.Message;
                entity.Status = StatusType.ERROR;
                entity.Erro = ex.Message;
                await serviceLoteStatus.SaveAsync(entity);
            }
            catch (BaseException ex)
            {
                entity.CodigoErro = ErroType.BASE_EXCEPTION;
                entity.Status = StatusType.ERROR;
                entity.Erro = ex.Message;
                await serviceLoteStatus.SaveAsync(entity);
            }
            catch (Exception ex)
            {
                entity.CodigoErro = ErroType.UNKNOWN;
                entity.Status = StatusType.ERROR;
                entity.Erro = ex.Message;
                await serviceLoteStatus.SaveAsync(entity);
            }
        }
    }
}
