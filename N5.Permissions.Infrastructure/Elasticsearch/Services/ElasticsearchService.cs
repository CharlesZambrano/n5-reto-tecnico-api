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

        public ElasticsearchService() { }

        public ElasticsearchService(ElasticsearchClient elasticClient, ILogger<ElasticsearchService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

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
                    _logger?.LogError($"Error indexing permission in Elasticsearch: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception when indexing permission in Elasticsearch: {ex.Message}");
            }
        }

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
                    _logger?.LogError($"Error getting Elasticsearch permissions: {response.DebugInformation}");
                    return Enumerable.Empty<EsPermissionDoc>();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception getting Elasticsearch permissions: {ex.Message}");
                return Enumerable.Empty<EsPermissionDoc>();
            }
        }

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
                    _logger?.LogError($"Error looking up permissions in Elasticsearch: {response.DebugInformation}");
                    return Enumerable.Empty<EsPermissionDoc>();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception when searching for permissions in Elasticsearch: {ex.Message}");
                return Enumerable.Empty<EsPermissionDoc>();
            }
        }
    }
}
