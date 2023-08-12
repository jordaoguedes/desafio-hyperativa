using DesafioHyperativa.Entities;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHyperativa.Controller;

[Authorize]
public class CartaoController : ControllerBase
{
    private readonly ICartaoService _serviceCartao;

    public CartaoController(ICartaoService serviceCartao)
    {
        _serviceCartao = serviceCartao;
    }

    [HttpPost("InserirCartao")]
    public async Task<ActionResult<Lote>> InserirCartao(string cartao)
    {
        try
        {
            if (string.IsNullOrEmpty(cartao))
                return NotFound("Preencha o cartão.");

            await _serviceCartao.InserirCartao(cartao);

            return Ok(new { message = "Inserido com sucesso." });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno no servidor. Erro: {ex.Message}");
        }
    }

    [HttpPost("InserirCriptografado")]
    public async Task<ActionResult<Lote>> InserirCriptografado(string cartaoCriptografado)
    {
        try
        {
            if (string.IsNullOrEmpty(cartaoCriptografado))
                return NotFound("Preencha o cartão.");

            await _serviceCartao.InserirCartaoCriptografado(cartaoCriptografado);

            return Ok(new { message = "Inserido com sucesso." });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno no servidor. Erro: {ex.Message}");
        }
    }

    [HttpGet("GetIdCartao")]
    public async Task<ActionResult<Lote>> GetIdCartao(string numeroCartao)
    {
        try
        {
            if (string.IsNullOrEmpty(numeroCartao))
                return NotFound("Preencha o cartão.");

            int id = await _serviceCartao.GetIdByNumeroCartao(numeroCartao);

            return Ok(new { id = id });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno no servidor. Erro: {ex.Message}");
        }
    }
    [HttpGet("GetIdCartaoCripografado")]
    public async Task<ActionResult<Lote>> GetIdCartaoCripografado(string numeroCartaoCripografado)
    {
        try
        {
            if (string.IsNullOrEmpty(numeroCartaoCripografado))
                return NotFound("Preencha o cartão.");

            int id = await _serviceCartao.GetIdByNumeroCartaoCriptografado(numeroCartaoCripografado);

            return Ok(new { id = id });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno no servidor. Erro: {ex.Message}");
        }
    }
}
