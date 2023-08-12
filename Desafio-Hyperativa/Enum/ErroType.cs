using System.ComponentModel;

namespace DesafioHyperativa.Enum;

public enum ErroType
{
    [Description("Bad Request")]
    BAD_REQUEST = 1,
    [Description("Base Exception")]
    BASE_EXCEPTION = 2,
    [Description("Erro Genérico")]
    UNKNOWN = 3
    
    
    

}
