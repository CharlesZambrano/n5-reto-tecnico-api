// *? n5-reto-tecnico-api/N5.Permissions.Api/Program.cs

using Microsoft.EntityFrameworkCore;
using N5.Permissions.Infrastructure.Persistence;
using N5.Permissions.Infrastructure.Repositories;
using N5.Permissions.Domain.Interfaces.Repositories;
using N5.Permissions.Domain.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;
using N5.Permissions.Infrastructure.Elasticsearch.Services;
using Elastic.Clients.Elasticsearch;
using System.Text.Json.Serialization;
using N5.Permissions.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Elasticsearch
var elasticsearchUri = builder.Configuration["Elasticsearch:Uri"];
if (string.IsNullOrEmpty(elasticsearchUri))
{
    throw new ArgumentNullException(nameof(elasticsearchUri), "Elasticsearch URI no está configurado en appsettings.json");
}

var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUri))
    .DefaultIndex("permissions");

builder.Services.AddSingleton(new ElasticsearchClient(settings));
builder.Services.AddSingleton<ElasticsearchService>();

// Agregar servicios
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Registrar MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    Assembly.Load("N5.Permissions.Application")
));

// Registrar AutoMapper para que se descubran automáticamente los perfiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configurar SQL Server con EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositorios en la inyección de dependencias
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "N5 Permissions API", Version = "v1" });
});

var app = builder.Build();

// Configurar el pipeline HTTP

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "N5 Permissions API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Insertar el middleware de excepciones globales
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
