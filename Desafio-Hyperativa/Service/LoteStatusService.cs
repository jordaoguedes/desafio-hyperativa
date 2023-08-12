using DesafioHyperativa.Entities;
using DesafioHyperativa.Enum;
using DesafioHyperativa.Exceptions;
using DesafioHyperativa.Repositories.Contracts;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Contract.Base;
using DesafioHyperativa.Util;

namespace DesafioHyperativa.Service;

public class LoteStatusService : Service<LoteStatus>, ILoteStatusService
{
    private readonly ILoteStatusRepository _repositoryLoteStatus;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private StatusType _status;
    private bool _hasItemEmEspera;

    public LoteStatusService(ILoteStatusRepository repositoryLoteStatus, IServiceScopeFactory serviceScopeFactory) : base(repositoryLoteStatus)
    {
        _status = StatusType.WAITING;
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

        await ProcessarInBackGround();

        return loteStatus.Guid.ToString();
    }

    public async Task<LoteStatus?> VerificarGuid(string guid)
    {
        return await _repositoryLoteStatus.GetByGuid(guid);
    }

    public Task<IList<LoteStatus>> GetAll()
    {
        return this._repositoryLoteStatus.GetAll();
    }

    private async Task ProcessarInBackGround()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var serviceLote = scope.ServiceProvider.GetRequiredService<ICartaoLoteService>();
            var serviceLoteStatus = scope.ServiceProvider.GetRequiredService<ILoteStatusService>();
            List<LoteStatus> lstLoteStatus = new List<LoteStatus>();

            if (_status == StatusType.IN_PROGRESS)
            {
                _hasItemEmEspera = true;
                return;
            }

            _status = StatusType.IN_PROGRESS;

            do
            {
                var lst = await serviceLoteStatus.GetAll();

                if (lst.Count() == 0)
                {
                    _hasItemEmEspera = false;
                    _status = StatusType.WAITING;
                }

                foreach (var item in lst)
                {
                    item.Status = StatusType.IN_PROGRESS;
                    await serviceLoteStatus.SaveAsync(item);

                    try
                    {
                        await serviceLote.ProcessarSalvarArquivo(Extension.DecryptBytesToFormFile(item.File));
                        item.Status = StatusType.FINISHED;
                        await serviceLoteStatus.SaveAsync(item);
                    }
                    catch (BadRequestException ex)
                    {
                        item.Erro = ex.Message;
                        item.Status = StatusType.ERROR;
                        item.Erro = ex.Message;
                        await serviceLoteStatus.SaveAsync(item);
                    }
                    catch (BaseException ex)
                    {
                        item.CodigoErro = ErroType.BASE_EXCEPTION;
                        item.Status = StatusType.ERROR;
                        item.Erro = ex.Message;
                        lstLoteStatus.Add(item);
                    }
                    catch (Exception ex)
                    {
                        item.CodigoErro = ErroType.UNKNOWN;
                        item.Status = StatusType.ERROR;
                        item.Erro = ex.Message;
                        lstLoteStatus.Add(item);
                    }
                }
            }
            while (_hasItemEmEspera);
            if (lstLoteStatus.Count > 0)
                await serviceLoteStatus.SaveRangeAsync(lstLoteStatus);
        }
    }
}
