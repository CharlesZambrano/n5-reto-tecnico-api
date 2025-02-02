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
        _testHost = TestHostBuilder.CreateTestHost();
        _testHost.Start();

        TestHostBuilder.SeedData(_testHost);

        var server = _testHost.GetTestServer();
        _client = server.CreateClient();
    }

    private async Task<string> GetValidTokenAsync()
    {
        var payload = new { Username = "admin", Password = "AdminPass123" };
        var loginContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);

        loginResponse.EnsureSuccessStatusCode();

        var jsonString = await loginResponse.Content.ReadAsStringAsync();
        var loginObj = JsonSerializer.Deserialize<LoginResult>(jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return loginObj?.Token ?? string.Empty;
    }

    private class LoginResult
    {
        public string Token { get; set; } = string.Empty;
    }

    [Fact]
    public async Task GetPermissions_Returns_Success()
    {
        var token = await GetValidTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/permission");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreatePermission_Returns_Created()
    {
        var token = await GetValidTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var payload = new
        {
            EmployeeName = "Alice",
            EmployeeSurname = "Smith",
            PermissionTypeId = 1,
            PermissionDate = "2025-02-01"
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/permission", content);

        Assert.Equal(201, (int)response.StatusCode);
    }
}
