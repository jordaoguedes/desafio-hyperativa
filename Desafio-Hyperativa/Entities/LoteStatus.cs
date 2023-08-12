using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Enum;
using System.Reflection.Metadata.Ecma335;

namespace DesafioHyperativa.Entities;

public class LoteStatus : BaseEntity
{
    public string Guid { get; set; } = string.Empty;
    public StatusType Status { get; set; }
    public string Erro { get; set; } = string.Empty;
    public ErroType? CodigoErro { get; set; }
    public byte[] File { get; set; }
}


