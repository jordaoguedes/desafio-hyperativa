using DesafioHyperativa.Context.Configuration.BaseConfiguration;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioHyperativa.Context.Configuration;

public class CartaoLoteoConfiguration : BaseEntityConfiguration<CartaoLote>
{
    public override void Configure(EntityTypeBuilder<CartaoLote> builder)
    {
        base.Configure(builder);
        builder.ToTable("cartao_lote");

        builder.HasOne(x => x.Cartao)
       .WithMany(x => x.ListCartao)
       .HasForeignKey(x => x.CartaoId)
       .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Lote)
        .WithMany(x => x.ListLote)
        .HasForeignKey(x => x.LoteId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
