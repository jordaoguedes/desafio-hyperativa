using DesafioHyperativa.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace DesafioHyperativa.Context;

public class ContextDb : DbContext
{
    public ContextDb(DbContextOptions<ContextDb> option) : base(option)
    {          
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public DbSet<Lote> Lote { get; set; }
    public DbSet<Cartao> Cartao { get; set; }
    public DbSet<LoteStatus> LoteStatus { get; set; }
}
