using DesafioHyperativa.Entities.Base;

namespace DesafioHyperativa.Entities;

public class Cartao : BaseEntity
{
    public string NumeroCartao { get; set; } = string.Empty;
    public virtual char IdentificadorLinha { get; set; }
    public virtual int NumeracaoLote { get; set; }
    public virtual List<CartaoLote> ListCartao { get; set; } = new();
}
