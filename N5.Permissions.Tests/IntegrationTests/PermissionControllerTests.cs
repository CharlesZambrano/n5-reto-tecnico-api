// *? n5-reto-tecnico-api/N5.Permissions.Tests/IntegrationTests/PermissionControllerTests.cs

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.TestHost;
using N5.Permissions.Tests.IntegrationTests;
using Xunit;

public class PermissionControllerTests
{
    private readonly HttpClient _client;
    private readonly IHost _testHost;

    public PermissionControllerTests()
    {
        // 1) Creamos el host y lo arrancamos
        _testHost = TestHostBuilder.CreateTestHost();
        _testHost.Start();

        // 2) Sembramos datos (permisoType #1 y #2, etc.)
        TestHostBuilder.SeedData(_testHost);

        var server = _testHost.GetTestServer();
        _client = server.CreateClient();
    }

    // Método auxiliar para obtener un token real
    private async Task<string> GetValidTokenAsync()
    {
        // Llamamos a /api/auth/login con credenciales de appsettings.json (p.e. "user"/"UserPass123")
        var payload = new { Username = "admin", Password = "AdminPass123" };
        var loginContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);

        // Verificamos que no haya fallado
        loginResponse.EnsureSuccessStatusCode();

        // Leemos el JSON => { "token":"..." }
        var jsonString = await loginResponse.Content.ReadAsStringAsync();
        var loginObj = JsonSerializer.Deserialize<LoginResult>(jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Retornamos solo la cadena del token
        return loginObj?.Token ?? string.Empty;
    }

    // DTO para deserializar la respuesta del login
    private class LoginResult
    {
        public string Token { get; set; } = string.Empty;
    }

    [Fact]
    public async Task GetPermissions_Returns_Success()
    {
        // 1) Obtenemos un token
        var token = await GetValidTokenAsync();

        // 2) Lo configuramos en el header Authorization
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // 3) Ahora sí GET /api/permission
        var response = await _client.GetAsync("/api/permission");

        // 4) Debe dar 200 OK
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreatePermission_Returns_Created()
    {
        // 1) Obtenemos un token
        var token = await GetValidTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // 2) Armamos el payload
        var payload = new
        {
            EmployeeName = "Alice",
            EmployeeSurname = "Smith",
            PermissionTypeId = 1,
            PermissionDate = "2025-02-01"
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        // 3) POST a /api/permission
        var response = await _client.PostAsync("/api/permission", content);

        // 4) Verificamos que sea 201 Created
        Assert.Equal(201, (int)response.StatusCode);
    }
}
