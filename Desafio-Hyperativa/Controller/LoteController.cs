using DesafioHyperativa.DTOs;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Enum;
using DesafioHyperativa.Exceptions;
using DesafioHyperativa.Repositories.Contracts;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DesafioHyperativa.Controller;

[Authorize]
public class LoteController : ControllerBase
{
    private readonly ILoteStatusService _serviceLoteStatus;

    public LoteController(ILoteStatusService serviceLoteStatus)
    {
        _serviceLoteStatus = serviceLoteStatus;
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

            if(fileExtension != ".txt")
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
                return NotFound();

            var loteStatus = await _serviceLoteStatus.VerificarGuid(guid);

            if(loteStatus == null)
                return NotFound();

            if (loteStatus.Status == StatusType.ERROR)
            {
                if (loteStatus.CodigoErro == ErroType.BAD_REQUEST || loteStatus.CodigoErro == ErroType.BASE_EXCEPTION)
                    return BadRequest(loteStatus.Erro);
                else
                    return StatusCode(500, $"Erro: {loteStatus.Erro}");             
            }

            return Ok(loteStatus.Status.ToString());
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno no servidor. Erro: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Lote>> InserirCartao()
    {
        var token = TokenGenerator.GenerateToken();

        return Ok(token);
    }
}
