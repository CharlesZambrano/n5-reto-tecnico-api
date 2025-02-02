// *? n5-reto-tecnico-api/N5.Permissions.Tests/IntegrationTests/TestHostBuilder.cs

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using N5.Permissions.Api.Middlewares;
using N5.Permissions.Infrastructure.Persistence;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Domain.Interfaces.Repositories;
using N5.Permissions.Infrastructure.Elasticsearch.Services;
using N5.Permissions.Infrastructure.Repositories;
using Elastic.Clients.Elasticsearch;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace N5.Permissions.Tests.IntegrationTests
{
    public static class TestHostBuilder
    {
        public static IHost CreateTestHost()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.Configure((ctx, app) =>
                    {
                        app.UseMiddleware<ExceptionHandlingMiddleware>();
                        app.UseRouting();

                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });

                    webBuilder.ConfigureServices((ctx, services) =>
                    {
                        services.AddSingleton<IConfiguration>(ctx.Configuration);

                        var elasticsearchUri = "http://localhost:9200";
                        var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUri))
                            .DefaultIndex("permissions");
                        services.AddSingleton(new ElasticsearchClient(settings));
                        services.AddSingleton<ElasticsearchService>();

                        services.AddAuthentication("FakeScheme")
                            .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(
                                "FakeScheme", options => { });


                        services.AddScoped<TokenService>();
                        services.AddScoped<UserService>();

                        services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                                options.JsonSerializerOptions.WriteIndented = true;
                            });

                        services.AddMediatR(cfg =>
                            cfg.RegisterServicesFromAssemblies(
                                Assembly.Load("N5.Permissions.Application")
                            )
                        );

                        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestPermissionsDb");
                        });

                        services.AddScoped<IPermissionRepository, PermissionRepository>();
                        services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
                        services.AddScoped<IUnitOfWork, UnitOfWork>();

                        services.AddControllers();
                    });
                });

            return builder.Build();
        }

        public static void SeedData(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.Database.EnsureCreated();

            var vacationType = new PermissionType
            {
                Description = "Vacaciones",
                Code = "VAC",
                Permissions = new List<Permission>()
            };
            var medicalType = new PermissionType
            {
                Description = "Permiso Médico",
                Code = "PME",
                Permissions = new List<Permission>()
            };

            db.PermissionTypes.AddRange(vacationType, medicalType);

            var permissions = new[]
            {
                new Permission {
                    EmployeeName = "Alice",
                    EmployeeSurname = "Smith",
                    PermissionType = vacationType,
                    PermissionDate = new DateTime(2025,02,01)
                },
                new Permission {
                    EmployeeName = "Bob",
                    EmployeeSurname = "Johnson",
                    PermissionType = medicalType,
                    PermissionDate = new DateTime(2025,02,02)
                }
            };

            db.Permissions.AddRange(permissions);
            db.SaveChanges();
        }
    }

    public class FakeAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public FakeAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, "FakeScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "FakeScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
