// *? n5-reto-tecnico-api/N5.Permissions.Tests/IntegrationTests/AuthControllerTests.cs

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.TestHost;
using N5.Permissions.Tests.IntegrationTests;
using Xunit;

public class AuthControllerTests
{
    private readonly HttpClient _client;

    public AuthControllerTests()
    {
        // 1) Creamos el host
        IHost testHost = TestHostBuilder.CreateTestHost();
        // 2) Arrancamos
        testHost.Start();

        // 3) Sembramos datos (si deseas semilla global, hazlo una sola vez).
        //    O hazlo en cada test, como prefieras.
        TestHostBuilder.SeedData(testHost);

        // 4) Obtenemos el TestServer y creamos HttpClient
        var server = testHost.GetTestServer();
        _client = server.CreateClient();
    }

    [Fact]
    public async Task Login_Returns_Token()
    {
        var payload = new
        {
            Username = "admin",
            Password = "AdminPass123"
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("token", responseString);
    }
}
