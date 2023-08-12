namespace DesafioHyperativa.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public DateTime ExpireDate { get; set; }
}
