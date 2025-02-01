// *? n5-reto-tecnico-api/N5.Permissions.Api/Controllers/AuthController.cs

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly UserService _userService;

    public AuthController(TokenService tokenService, UserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var (isValid, role) = _userService.ValidateCredentials(request.Username, request.Password);

        if (!isValid)
            return Unauthorized("Invalid credentials.");

        var token = _tokenService.GenerateToken(request.Username, role);
        return Ok(new { Token = token });
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}