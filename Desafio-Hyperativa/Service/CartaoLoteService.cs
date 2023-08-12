using DesafioHyperativa.Entities;
using DesafioHyperativa.Exceptions;
using DesafioHyperativa.Repositories.Contracts;
using DesafioHyperativa.Service.Contract;
using DesafioHyperativa.Service.Contract.Base;
using DesafioHyperativa.Util;
using Newtonsoft.Json.Schema;
using System.Globalization;
using System.Text;

namespace DesafioHyperativa.Service;

public class CartaoLoteService : Service<CartaoLote>, ICartaoLoteService
{
    private readonly ICartaoRepository _repositoryCartao;
    public CartaoLoteService(IRepository<CartaoLote> repository, ICartaoRepository repositoryCartao) : base(repository)
    {
        this._repositoryCartao = repositoryCartao;
    }

    #region Methods

    //public override void Validate(CartaoLote cartaoLote)
    //{ 
    //    if(lote == null)
    //        throw new Exception("Não existe informações de cartao lote para salvar.");

    //    var lstCartoes = lote

    //    var lst = lote..Select(x => x.NumeroCartao).ToList();

    //    var lstCartao = repositoryCartao.GetCartoesJaInseridos(lst).ToList();

    //    if (lst.Count > 0)
    //        throw new Exception($"O(s) cartão(ões) : {String.Join(", ", lst)} já existem na base de dados.");
    //}
    public async Task ProcessarSalvarArquivo(IFormFile file)
    {
        try
        {
            List<CartaoLote> cartaoLote = PreencherDados(file);

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

    private async Task SaveRangeAsync(List<CartaoLote> cartaoLote)
    {
        List<string> lstCartaoOld = cartaoLote.Select(x => x.Cartao.NumeroCartao).ToList();

        var lstCartao = _repositoryCartao.GetCartoesJaInseridos(lstCartaoOld);

        if (lstCartao.Count > 0)
        {
            foreach (var item in cartaoLote)
            {
                if (lstCartao.Any(x => x.NumeroCartao == item.Cartao?.NumeroCartao))
                {
                    item.CartaoId = lstCartao.Where(x => x.NumeroCartao == item.Cartao?.NumeroCartao).First().Id;
                    item.Cartao = null;
                }
            }
        }

        await this.Repository.SaveRangeAsync(cartaoLote);
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
