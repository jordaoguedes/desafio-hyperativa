using DesafioHyperativa.Context.Configuration.BaseConfiguration;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioHyperativa.Context.Configuration;

public class CartaoConfiguration : BaseEntityConfiguration<Cartao>
{
    public override void Configure(EntityTypeBuilder<Cartao> builder)
    {
        base.Configure(builder);

        builder.ToTable("cartao");
        builder.HasIndex(x => x.NumeroCartao).IsUnique();

        builder.Property(x => x.NumeroCartao).IsRequired();

        builder.Ignore(x => x.IdentificadorLinha);
        builder.Ignore(x => x.NumeracaoLote);
    }
}
