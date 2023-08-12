using DesafioHyperativa.Context.Configuration.BaseConfiguration;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioHyperativa.Context.Configuration;

public class LoteStatusConfiguration : BaseEntityConfiguration<LoteStatus>
{
    public override void Configure(EntityTypeBuilder<LoteStatus> builder)
    {
        base.Configure(builder);
        builder.ToTable("lote_status");

        builder.Property(x => x.Guid).IsRequired();
        //builder.Property(x => x.File).IsRequired();

        builder.HasIndex(x => x.Guid).IsUnique();

    }
}
