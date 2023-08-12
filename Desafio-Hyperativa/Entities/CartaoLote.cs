using DesafioHyperativa.Entities.Base;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace DesafioHyperativa.Entities;

public class CartaoLote : BaseEntity
{
    public Lote? Lote { get; set; }
    public int LoteId { get; set; }
    public Cartao? Cartao { get; set; }
    public int CartaoId { get; set; }
}
