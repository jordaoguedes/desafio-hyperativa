using DesafioHyperativa.Entities;
using DesafioHyperativa.Repository.Context.Configuration.BaseConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioHyperativa.Repository.Context.Configuration;

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
