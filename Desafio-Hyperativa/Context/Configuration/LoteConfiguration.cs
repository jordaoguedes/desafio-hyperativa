using DesafioHyperativa.Context.Configuration.BaseConfiguration;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioHyperativa.Context.Configuration;

public class LoteConfiguration : BaseEntityConfiguration<Lote>
{
    public override void Configure(EntityTypeBuilder<Lote> builder)
    {
        base.Configure(builder);
        builder.ToTable("lote");

        builder.Property(x => x.Nome).IsRequired();
        builder.Property(x => x.RegistroLote).IsRequired();

        builder.Property(x => x.Data)
               .IsRequired()
               .HasConversion<DateTime>()
               .HasColumnType("timestamp without time zone")
               .HasDefaultValueSql("current_timestamp")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.QuantidadeRegistro).IsRequired();
    }
}
