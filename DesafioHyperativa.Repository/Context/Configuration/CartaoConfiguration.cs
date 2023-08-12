using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Repository.Context.Configuration.BaseConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioHyperativa.Repository.Context.Configuration;

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
