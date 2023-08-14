using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.Util;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Contract.Base;
using DesafioHyperativa.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace DesafioHyperativa.Service;

public class CartaoService : Service<Cartao>, ICartaoService
{
    private readonly ICartaoRepository _repositoryCartao;
    private readonly ILogger<CartaoService> _logger;
    public CartaoService(ICartaoRepository repositoryCartao, ILogger<CartaoService> logger) : base(repositoryCartao)
    {
        _repositoryCartao = repositoryCartao;
        _logger = logger;
    }

    #region Methods
    public async Task InserirCartao(string numeroCartao)
    {
        try
        {
            await InsertCartao(new Cartao { NumeroCartao = Extension.Encrypt(numeroCartao) });
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception) 
        {
            throw;
        }
    }

    public async Task InserirCartaoCriptografado(string numeroCartaoCriptografado)
    {
        try
        {

            await InsertCartao(new Cartao {NumeroCartao = numeroCartaoCriptografado });
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new BadRequestException("Formato inválido.");
        }
    }

    public async Task<int> GetIdByNumeroCartao(string numeroCartao)
    {
        try
        {
            var numeroCripografado = Extension.Encrypt(numeroCartao);
            await Validate(new Cartao { NumeroCartao = numeroCripografado });
            return await _repositoryCartao.GetIdByNumeroAsync(numeroCripografado);
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception) 
        {
            throw;
        }
    }

    public async Task<int> GetIdByNumeroCartaoCriptografado(string numeroCartaoCriptografado)
    {
        await Validate(new Cartao { NumeroCartao = numeroCartaoCriptografado });
        return await _repositoryCartao.GetIdByNumeroAsync(numeroCartaoCriptografado);
    }
    private async Task InsertCartao(Cartao entity)
    {
        var cartao = await _repositoryCartao.GetCartaoByNumero(entity.NumeroCartao);

        if (cartao == null)
            await this.SaveAsync(entity);
        else
            throw new BadRequestException("Cartão Informado já existe na base de dados.");
    }
    public async override Task Validate(Cartao entity)
    {
        try
        {
            var cartaoDecript = Extension.Decrypt(entity.NumeroCartao);

            if (cartaoDecript.Trim().Length != 16)
                throw new BadRequestException("O cartão deve ter 16 caracteres");

            if(!Extension.VerificarSomenteNumero(cartaoDecript.Trim()))
                throw new BadRequestException("O cartão deve ser uma sequencia de 16 caracteres somente com numero");

            await base.Validate(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}
