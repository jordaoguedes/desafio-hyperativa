using Newtonsoft.Json;

namespace DesafioHyperativa.Service.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string mensagem) : base(mensagem)
    {
        _Message = JsonConvert.SerializeObject(new { message = mensagem });
    }
}
