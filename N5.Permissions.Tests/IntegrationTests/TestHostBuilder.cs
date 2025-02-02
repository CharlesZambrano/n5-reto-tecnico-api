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
                    // Carga (si gustas) la configuración real (appsettings.json)
                    // Pero ya no la usaremos para JWT, pues haremos un fake scheme
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.Configure((ctx, app) =>
                    {
                        // Middleware de excepción, routing y endpoints
                        app.UseMiddleware<ExceptionHandlingMiddleware>();
                        app.UseRouting();

                        app.UseAuthentication(); // Fake scheme
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });

                    // En lugar de AddJwtBearer, usamos un fake scheme
                    webBuilder.ConfigureServices((ctx, services) =>
                    {
                        // Registramos config (por si la necesitas en otras partes)
                        services.AddSingleton<IConfiguration>(ctx.Configuration);

                        // Elasticsearch (si lo deseas, igual)
                        var elasticsearchUri = "http://localhost:9200";
                        var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUri))
                            .DefaultIndex("permissions");
                        services.AddSingleton(new ElasticsearchClient(settings));
                        services.AddSingleton<ElasticsearchService>();

                        // FAKE AUTH SCHEME
                        // Registramos un handler que siempre "logea" con Rol = "Administrator"
                        services.AddAuthentication("FakeScheme")
                            .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(
                                "FakeScheme", options => { });

                        // Quitamos completamente la config de AddJwtBearer, 
                        // ya que no la usaremos en test

                        // Tus servicios reales
                        services.AddScoped<TokenService>();
                        services.AddScoped<UserService>();

                        services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                                options.JsonSerializerOptions.WriteIndented = true;
                            });

                        // MediatR
                        services.AddMediatR(cfg =>
                            cfg.RegisterServicesFromAssemblies(
                                Assembly.Load("N5.Permissions.Application")
                            )
                        );

                        // AutoMapper
                        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                        // InMemory DB
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestPermissionsDb");
                        });

                        // Repos
                        services.AddScoped<IPermissionRepository, PermissionRepository>();
                        services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
                        services.AddScoped<IUnitOfWork, UnitOfWork>();

                        // Controllers
                        services.AddControllers();
                    });
                });

            return builder.Build();
        }

        public static void SeedData(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Crea la DB in-memory si no existe
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

    // Handler de autenticación que simula un usuario con rol "Administrator"
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
            // Creamos un principal con la Role = "Administrator" para que `[Authorize(Roles="User,Administrator")]` pase.
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
