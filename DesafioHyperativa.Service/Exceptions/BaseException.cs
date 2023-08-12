using Newtonsoft.Json;

namespace DesafioHyperativa.Service.Exceptions;

public class BaseException : Exception
{
    public string _Message;

    public override string Message
    {
        get { return _Message; }
    }

    public BaseException(string mensagem) : base(mensagem)
    {
        _Message = mensagem;
    }
}
