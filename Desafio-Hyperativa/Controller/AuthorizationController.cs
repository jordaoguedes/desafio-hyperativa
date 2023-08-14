using DesafioHyperativa.DTOs;
using DesafioHyperativa.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioHyperativa.Controller;

[AllowAnonymous]
[Route("Token")]

public class AuthorizationController : ControllerBase
{
    private readonly ILogger<AuthorizationController> _logger;
    public AuthorizationController(ILogger<AuthorizationController> logger)
    {
        _logger = logger;
    }
    [HttpPost]
    public ActionResult<TokenDto> Authorize()
    {
        var token = TokenGenerator.GenerateToken();

        return Ok(token);
    }
}
