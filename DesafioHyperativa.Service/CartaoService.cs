using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.Util;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Contract.Base;
using DesafioHyperativa.Service.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioHyperativa.Service;

public class CartaoService : Service<Cartao>, ICartaoService
{
    private readonly ICartaoRepository _repositoryCartao;
    public CartaoService(ICartaoRepository repositoryCartao) : base(repositoryCartao)
    {
        _repositoryCartao = repositoryCartao;
    }

    #region Methods
    public async Task InserirCartao(string numeroCartao)
    {
        if (numeroCartao.Length != 16)
            throw new BadRequestException("O cartão deve ter 16 caracteres");

        var cartao = new Cartao();
        cartao.NumeroCartao = Extension.Encrypt(numeroCartao);

        await this.SaveAsync(cartao);

    }

    public async Task InserirCartaoCriptografado(string numeroCartaoCriptografado)
    {
        try 
        {
            var numeroCartao = Extension.Decrypt(numeroCartaoCriptografado);

            if (numeroCartao.Length != 16)
                throw new BadRequestException("O cartão deve ter 16 caracteres");
        }
        catch (Exception)
        {
            throw new BadRequestException("Formato inválido.");
        }
        
        var cartao = new Cartao();
        cartao.NumeroCartao = numeroCartaoCriptografado;

        await this.SaveAsync(cartao);
    }

    public async Task<int> GetIdByNumeroCartao(string numeroCartao)
    {
        if (numeroCartao.Trim().Length != 16)
            throw new BadRequestException("O cartão deve ter 16 caracteres");

        var numeroCripografado = Extension.Encrypt(numeroCartao);  

        return await _repositoryCartao.GetIdByNumeroAsync(numeroCripografado);
    }

    public async Task<int> GetIdByNumeroCartaoCriptografado(string numeroCartaoCriptografado)
    {
        return await _repositoryCartao.GetIdByNumeroAsync(numeroCartaoCriptografado);
    }
    public async override Task Validate(Cartao entity)
    {
        var cartao = await _repositoryCartao.GetCartaoByNumero(entity.NumeroCartao);

        if (cartao == null)
            await _repositoryCartao.SaveAsync(entity);
        else
            throw new BadRequestException("Cartão Informado já existe na base de dados.");
    }
    #endregion
}
