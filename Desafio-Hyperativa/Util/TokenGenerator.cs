using DesafioHyperativa.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace DesafioHyperativa.Util;

public static class TokenGenerator
{
    public static readonly string secret = "241A27DDF7E1814D73908517A0EF449DACFEF2367615BB99305449200CB5E25B";

    public static TokenDto GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);

        var identity = new ClaimsIdentity(new[] {       
              new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(6).ToString()),
        });

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new TokenDto
        {
            AccessToken = tokenString,
            CreateDate = DateTime.UtcNow,
            ExpireDate = DateTime.UtcNow.AddHours(6)
        };
    }
}
