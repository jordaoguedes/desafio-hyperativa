namespace DesafioHyperativa.Entities.Base;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? DataRegistro { get; set; }
    public DateTime? DataUpdate { get; set; }
}
