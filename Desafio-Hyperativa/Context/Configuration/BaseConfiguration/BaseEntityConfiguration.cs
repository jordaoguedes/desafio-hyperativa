using DesafioHyperativa.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioHyperativa.Context.Configuration.BaseConfiguration;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder) 
    { 
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnType("int8")
               .HasColumnOrder(0);

        builder.Property(x => x.DataRegistro)
               .HasColumnOrder(1)
               .HasConversion<DateTime>()
               .HasColumnType("timestamp without time zone")
               .HasDefaultValueSql("current_timestamp")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.DataUpdate)
               .HasColumnOrder(2)
               .HasConversion<DateTime>()    
               .HasColumnType("timestamp without time zone")
               .HasDefaultValueSql("current_timestamp")
               .ValueGeneratedOnAdd();
    }
}
