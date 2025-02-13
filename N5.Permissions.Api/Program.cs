// *? n5-reto-tecnico-api/N5.Permissions.Api/Program.cs

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using N5.Permissions.Infrastructure.Persistence;
using N5.Permissions.Infrastructure.Repositories;
using N5.Permissions.Domain.Interfaces.Repositories;
using N5.Permissions.Domain.Interfaces;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using N5.Permissions.Api.Middlewares;
using Elastic.Clients.Elasticsearch;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Elasticsearch configuration
            var elasticsearchUri = builder.Configuration["Elasticsearch:Uri"];
            if (string.IsNullOrEmpty(elasticsearchUri))
            {
                throw new ArgumentNullException(nameof(elasticsearchUri), "Elasticsearch URI no está configurado en appsettings.json");
            }

            var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUri))
                .DefaultIndex("permissions");

            builder.Services.AddSingleton(new ElasticsearchClient(settings));
            builder.Services.AddSingleton<ElasticsearchService>();

            // JWT Configuration
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("Secret");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontendDev",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // Register custom services
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<UserService>();

            // Add Services
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            // MediatR registration
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                Assembly.GetExecutingAssembly(),
                Assembly.Load("N5.Permissions.Application")
            ));

            // AutoMapper registration
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Setting up SQL Server with EF Core
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositorios registration
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Swagger Configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "N5 Permissions API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT generado"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            // Setting up the HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "N5 Permissions API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontendDev");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
