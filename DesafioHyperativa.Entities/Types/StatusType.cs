using System.ComponentModel;

namespace DesafioHyperativa.Entities.Types;

public enum StatusType
{
    [Description("Aceito")]
    ACCEPTED = 1,
    [Description("Em Espera")]
    WAITING = 2,
    [Description("Em Progresso")]
    IN_PROGRESS = 3,
    [Description("Finalizado")]
    FINISHED = 4,
    [Description("Erro")]
    ERROR = 5
}
