// *? n5-reto-tecnico-api/N5.Permissions.Infrastructure/Elasticsearch/Services/ElasticsearchService.cs

using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Infrastructure.Elasticsearch.Services
{
    public class ElasticsearchService
    {
        private readonly ElasticsearchClient _elasticClient;
        private readonly ILogger<ElasticsearchService> _logger;
        private const string IndexName = "permissions";

        public ElasticsearchService(ElasticsearchClient elasticClient, ILogger<ElasticsearchService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            try
            {
                var document = new
                {
                    permission.Id,
                    permission.EmployeeName,
                    permission.EmployeeSurname,
                    permission.PermissionTypeId,
                    permission.PermissionDate,
                    PermissionTypeDescription = permission.PermissionType?.Description
                };

                var response = await _elasticClient.IndexAsync(document, idx => idx.Index(IndexName));

                if (!response.IsValidResponse)
                {
                    _logger.LogError($"Error indexing permission: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while indexing permission: {ex.Message}");
            }
        }

        // Se agrega el método SearchPermissionsAsync para buscar permisos en Elasticsearch
        public async Task<IEnumerable<Permission>> SearchPermissionsAsync(string query)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<Permission>(s => s
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
                    _logger.LogError($"Error searching permissions: {response.DebugInformation}");
                    return Enumerable.Empty<Permission>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while searching permissions: {ex.Message}");
                return Enumerable.Empty<Permission>();
            }
        }
    }
}
