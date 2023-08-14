using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.Util;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Contract.Base;
using DesafioHyperativa.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;

namespace DesafioHyperativa.Service;

public class CartaoLoteService : Service<CartaoLote>, ICartaoLoteService
{
    private readonly ICartaoRepository _repositoryCartao;
    private readonly ILoteRepository _repositoryLote;
    public CartaoLoteService(IRepository<CartaoLote> repository, ICartaoRepository repositoryCartao, ILoteRepository repositoryLote) : base(repository)
    {
        _repositoryCartao = repositoryCartao;
        _repositoryLote = repositoryLote;
    }

    #region Methods
    public async Task ProcessarSalvarArquivo(IFormFile file)
    {
        try
        {
            List<CartaoLote> cartaoLote = PreencherDados(file);
            await ValidateCompareOldValuesInDB(cartaoLote);
            await this.SaveRangeAsync(cartaoLote);
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BaseException(ex.Message);
        }
    }

    public async Task ValidateCompareOldValuesInDB(List<CartaoLote> lstEntities)
    {
        try
        {
            if (lstEntities == null)
                throw new Exception("Entidade a ser salva nula.");

            List<string> lstCartaoOld = lstEntities.Where(x => x.Cartao != null).Select(x => x.Cartao.NumeroCartao).ToList();

            var nome = lstEntities?.FirstOrDefault().Lote?.Nome;         
            
            if (string.IsNullOrEmpty(nome))
                throw new BadRequestException("Nome do lote não foi informado.");

            if (_repositoryLote.VerificarLoteExiste(nome))
                throw new Exception("Lote já cadastrado.");


            var lstCartao = await _repositoryCartao.GetCartoesJaInseridos(lstCartaoOld);

            if (lstCartao.Count > 0)
            {
                foreach (var item in lstEntities)
                {
                    if (lstCartao.Any(x => x.NumeroCartao == item.Cartao?.NumeroCartao))
                    {
                        item.CartaoId = lstCartao.Where(x => x.NumeroCartao == item.Cartao?.NumeroCartao).First().Id;
                        item.Cartao = null;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Private Methods
    private List<CartaoLote> PreencherDados(IFormFile file)
    {
        try
        {
            List<string> linhas = ReadFile(file);

            if (linhas.Count < 3)
                throw new BadRequestException("O arquivo deve conter pelo menos três linhas (Nome, Data, Lote e um Cartão).");

            List<CartaoLote> lote = LerLote(linhas);

            return lote;
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

    private List<CartaoLote> LerLote(List<string> linhas)
    {
        try
        {
            string primeiraLinha = linhas[0];
            string ultimaLinha = linhas[linhas.Count - 1];
            List<string> erros = new List<string>();

            var nome = primeiraLinha.Substring(0, 29).Trim();

            if (string.IsNullOrEmpty(nome))
                erros.Add("Nome");

            if (!DateTime.TryParseExact(primeiraLinha.Substring(29, 8).Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
                erros.Add("Data");

            var registroLote = primeiraLinha.Substring(37, 8).Trim();
            if (string.IsNullOrEmpty(registroLote))
                erros.Add("Lote");

            int.TryParse(primeiraLinha.Substring(45, 6).Trim(), out int qtdRegistros);

            if (qtdRegistros == 0)
                erros.Add("Quantidade de registros");

            string loteUltimaLinha = ultimaLinha.Substring(0, 8).Trim();

            if (string.IsNullOrEmpty(loteUltimaLinha))
                erros.Add("Última linha Lote");

            int.TryParse(ultimaLinha.Substring(8, 6).Trim(), out int qtdRegistrosUltimaLinha);

            if (qtdRegistrosUltimaLinha == 0)
                erros.Add("Última linha Quantidadade registros");

            if (erros.Any())
                throw new BadRequestException(string.Join(", ", erros) + " inválido(s).");

            if (!loteUltimaLinha.Equals(loteUltimaLinha))
                erros.Add("Lote");

            if (qtdRegistros != qtdRegistrosUltimaLinha)
                erros.Add("Quantidade de Registro");

            if (erros.Any())
                throw new BadRequestException(string.Join(", ", erros) + " da primeira e ultima linha não confere(m).");

            var lote = new Lote
            {
                Nome = nome,
                Data = data,
                RegistroLote = registroLote,
                QuantidadeRegistro = qtdRegistros
            };

            var lstCartoes = ReadDadosCartao(linhas, lote);

            if (lstCartoes.Count() != qtdRegistros)
                throw new BadRequestException("A quantidade de registros não confere com a quantidade de cartões.");

            return lstCartoes;
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

    private List<CartaoLote> ReadDadosCartao(List<string> linhas, Lote lote)
    {
        try
        {
            List<CartaoLote> lstCartaoLote = new List<CartaoLote>();
            List<string> erros = new List<string>();

            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            for (int i = 1; i < linhas.Count - 1; i++)
            {
                string linhaCartao = linhas[i];

                char? identificadorLinha = Convert.ToChar(linhaCartao.Substring(0, 1));

                if (identificadorLinha is null)
                    erros.Add($"Identificador da linha {i + 1}");

                int.TryParse(linhaCartao.Substring(1, 6), out int numeracaoLote);

                if (numeracaoLote == 0)
                    erros.Add($"Numeração Lote da linha {i + 1}");

                string numeroCartao = linhaCartao.Substring(7).Split("/")[0].Trim();

                if (string.IsNullOrEmpty(numeroCartao))
                    erros.Add($"Numero Cartão da linha {i + 1}");

                if (numeroCartao.Length != 16)
                    erros.Add($"O número de cartão da linha {i + 1} deve ter 16 caracteres");
                else if (!Extension.VerificarSomenteNumero(numeroCartao.Trim()))
                    throw new BadRequestException($"O cartão da linha {i + 1} deve ser uma sequencia de 16 caracteres somente com numero");

                Cartao cartao = new Cartao
                {
                    IdentificadorLinha = identificadorLinha.GetValueOrDefault(),
                    NumeracaoLote = numeracaoLote,
                    NumeroCartao = Extension.Encrypt(numeroCartao)
                };

                if (!dict.ContainsKey(numeroCartao))
                    dict.Add(numeroCartao, new List<string>() { $"{(i + 1)} ({identificadorLinha.GetValueOrDefault().ToString() + numeracaoLote.ToString()})" });
                else
                    dict[numeroCartao].Add($"{(i + 1)} ({identificadorLinha.GetValueOrDefault().ToString() + numeracaoLote.ToString()})");

                lstCartaoLote.Add(new CartaoLote
                {
                    Cartao = cartao,
                    Lote = lote
                });
            }

            foreach (var kvp in dict)
            {
                if (kvp.Value.Count > 1)
                {
                    erros.Add($"Existe cartões duplicados nas linhas {String.Join(", ", kvp.Value)}");
                }
            }

            if (erros.Any())
                throw new BadRequestException("Erro(s): " + string.Join(", ", erros));


            return lstCartaoLote;
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

    private List<string> ReadFile(IFormFile file)
    {
        try
        {
            using (var streamReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                List<string> linhas = new List<string>();
                string linha;
                while ((linha = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(linha))
                        linhas.Add(linha);
                }

                return linhas;
            }
        }
        catch (Exception ex)
        {
            throw new BaseException(ex.Message);
        }
    }
    #endregion
}
