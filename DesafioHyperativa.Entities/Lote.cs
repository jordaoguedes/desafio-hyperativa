using DesafioHyperativa.Entities.Base;

namespace DesafioHyperativa.Entities;

public class Lote : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string RegistroLote { get; set; } = string.Empty;
    public int QuantidadeRegistro { get; set; }
    public virtual List<CartaoLote> ListLote { get; set; } = new();
}

