using DesafioHyperativa.Entities.Types;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Exceptions;
using DesafioHyperativa.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHyperativa.Controller;

[Authorize]
public class InserirArquivoController : ControllerBase
{
    private readonly ILogger<InserirArquivoController> _logger;
    private readonly ILoteStatusService _serviceLoteStatus;

    public InserirArquivoController(ILoteStatusService serviceLoteStatus, ILogger<InserirArquivoController> logger)
    {
        _serviceLoteStatus = serviceLoteStatus;
        _logger = logger;
    }

    [HttpPost("ProcessarArquivo")]
    public async Task<IActionResult> ProcessarArquivo(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo foi enviado.");

            var fileName = Path.GetFileName(file.FileName);

            var fileExtension = Path.GetExtension(fileName);

            if (fileExtension != ".txt")
                return BadRequest("Arquivo deve ser TXT.");

            var guid = await _serviceLoteStatus.ProcessarArquivo(file);

            return Ok(guid);
        }

        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }

        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        catch (BaseException ex)
        {
            return StatusCode(500, $"Erro interno no servidor. Erro: {ex.Message}");
        }
    }

    [HttpGet("VerificarStatus")]
    public async Task<IActionResult> VerificarStatus(string guid)
    {
        try
        {
            if (string.IsNullOrEmpty(guid))
                return NotFound(new { message = "Guid obrigatório" });

            var loteStatus = await _serviceLoteStatus.VerificarGuid(guid);

            if (loteStatus == null)
                return NotFound(new { message = guid + " não encontrado." });

            if (loteStatus.Status == StatusType.ERROR)
            {
                if (loteStatus.CodigoErro == ErroType.BAD_REQUEST || loteStatus.CodigoErro == ErroType.BASE_EXCEPTION)
                    return BadRequest(new { message = loteStatus.Erro });
                else
                    return StatusCode(500, $"Erro: {loteStatus.Erro}");
            }

            return Ok(new { message = loteStatus.Status.ToDescription() });

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno no servidor. Erro: {ex.Message}");
        }
    }
}