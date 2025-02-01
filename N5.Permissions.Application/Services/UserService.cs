// *? n5-reto-tecnico-api/N5.Permissions.Application/Services/UserService.cs

using Microsoft.Extensions.Configuration;

public class UserService
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (bool isValid, string role) ValidateCredentials(string username, string password)
    {
        var users = _configuration.GetSection("Users").Get<List<UserCredentials>>();

        var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

        return user != null ? (true, user.Role) : (false, null);
    }
}

public class UserCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}