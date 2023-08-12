using Newtonsoft.Json;

namespace DesafioHyperativa.Service.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string mensagem) : base(mensagem)
    {
        _Message = JsonConvert.SerializeObject(new { message = mensagem });
    }
}
