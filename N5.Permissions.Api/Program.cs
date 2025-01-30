// *? n5-reto-tecnico-api/N5.Permissions.Api/Program.cs

using Microsoft.EntityFrameworkCore;
using N5.Permissions.Infrastructure.Persistence;
using N5.Permissions.Infrastructure.Repositories;
using N5.Permissions.Domain.Interfaces.Repositories;
using Microsoft.OpenApi.Models;
using System.Reflection;
using N5.Permissions.Infrastructure.Elasticsearch.Services;
using Elastic.Clients.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de Elasticsearch
var elasticsearchUri = builder.Configuration["Elasticsearch:Uri"];
if (string.IsNullOrEmpty(elasticsearchUri))
{
    throw new ArgumentNullException(nameof(elasticsearchUri), "Elasticsearch URI no est� configurado en appsettings.json");
}

var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUri))
    .DefaultIndex("permissions");

builder.Services.AddSingleton(new ElasticsearchClient(settings));
builder.Services.AddSingleton<ElasticsearchService>();


// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Configurar SQL Server con EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositorios en la Inyecci�n de Dependencias
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
