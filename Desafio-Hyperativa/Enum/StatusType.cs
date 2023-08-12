using System.ComponentModel;

namespace DesafioHyperativa.Enum;

public enum StatusType
{
    [Description("Aceito")]
    ACCEPTED = 1,
    [Description("Em Espera")]
    WAITING = 1,
    [Description("Em Progresso")]
    IN_PROGRESS = 2,
    [Description("Finalizado")]
    FINISHED = 3,
    [Description("Erro")]
    ERROR = 4,
}
