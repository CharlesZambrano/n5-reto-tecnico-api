// *? n5-reto-tecnico-api/N5.Permissions.Infrastructure/Elasticsearch/Services/ElasticsearchService.cs

using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Infrastructure.Elasticsearch.Models;

namespace N5.Permissions.Infrastructure.Elasticsearch.Services
{
    public class ElasticsearchService
    {
        private readonly ElasticsearchClient? _elasticClient;
        private readonly ILogger<ElasticsearchService>? _logger;
        private const string IndexName = "permissions";

        /// <summary>
        /// Constructor sin parámetros para que Moq pueda instanciar la clase.
        /// OJO: _elasticClient y _logger quedan en null si usas este constructor.
        /// </summary>
        public ElasticsearchService()
        {
            // Idealmente no se usaría en producción o se haría mock de una interfaz.
        }

        public ElasticsearchService(ElasticsearchClient elasticClient, ILogger<ElasticsearchService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        /// <summary>
        /// Indexa (o actualiza) un permiso en Elasticsearch,
        /// usando un documento "EsPermissionDoc" con la info necesaria.
        /// </summary>
        public async Task IndexPermissionAsync(Permission permission)
        {
            try
            {
                var doc = new EsPermissionDoc
                {
                    Id = permission.Id,
                    EmployeeName = permission.EmployeeName,
                    EmployeeSurname = permission.EmployeeSurname,
                    PermissionTypeId = permission.PermissionTypeId,
                    PermissionDate = permission.PermissionDate,
                    PermissionTypeDescription = permission.PermissionType?.Description,
                    PermissionTypeCode = permission.PermissionType?.Code
                };

                var response = await _elasticClient!.IndexAsync(
                    doc,
                    idx => idx.Index(IndexName).Id(doc.Id)
                );

                if (!response.IsValidResponse)
                {
                    _logger?.LogError($"Error al indexar el permiso en Elasticsearch: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Excepción al indexar el permiso en Elasticsearch: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todos los documentos de permisos desde Elasticsearch (EsPermissionDoc).
        /// </summary>
        public async Task<IEnumerable<EsPermissionDoc>> GetAllPermissionsAsync()
        {
            try
            {
                var response = await _elasticClient!.SearchAsync<EsPermissionDoc>(s => s
                    .Index(IndexName)
                    .Query(q => q.MatchAll(m => { }))
                    .Size(1000)
                );

                if (response.IsValidResponse)
                {
                    return response.Documents;
                }
                else
                {
                    _logger?.LogError($"Error al obtener permisos de Elasticsearch: {response.DebugInformation}");
                    return Enumerable.Empty<EsPermissionDoc>();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Excepción al obtener permisos de Elasticsearch: {ex.Message}");
                return Enumerable.Empty<EsPermissionDoc>();
            }
        }

        /// <summary>
        /// Busca permisos en Elasticsearch usando un término de búsqueda (full-text).
        /// </summary>
        public async Task<IEnumerable<EsPermissionDoc>> SearchPermissionsAsync(string query)
        {
            try
            {
                var response = await _elasticClient!.SearchAsync<EsPermissionDoc>(s => s
                    .Index(IndexName)
                    .Query(q => q
                        .QueryString(qs => qs
                            .Query(query)
                        )
                    )
                );

                if (response.IsValidResponse)
                {
                    return response.Documents;
                }
                else
                {
                    _logger?.LogError($"Error al buscar permisos en Elasticsearch: {response.DebugInformation}");
                    return Enumerable.Empty<EsPermissionDoc>();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Excepción al buscar permisos en Elasticsearch: {ex.Message}");
                return Enumerable.Empty<EsPermissionDoc>();
            }
        }
    }
}
