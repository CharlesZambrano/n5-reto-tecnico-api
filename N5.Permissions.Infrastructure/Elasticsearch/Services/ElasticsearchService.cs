// *? n5-reto-tecnico-api/N5.Permissions.Infrastructure/Elasticsearch/Services/ElasticsearchService.cs

using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using N5.Permissions.Domain.Entities;


namespace N5.Permissions.Infrastructure.Elasticsearch.Services
{
    public class ElasticsearchService
    {
        private readonly ElasticsearchClient _elasticClient;
        private readonly ILogger<ElasticsearchService> _logger;
        private const string IndexName = "permissions";

        public ElasticsearchService(IConfiguration configuration, ILogger<ElasticsearchService> logger)
        {
            _logger = logger;
            var uri = configuration["Elasticsearch:Uri"];

            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri), "Elasticsearch URI no está configurado en appsettings.json");
            }

            var settings = new ElasticsearchClientSettings(new Uri(uri))
                .DefaultIndex(IndexName);

            _elasticClient = new ElasticsearchClient(settings);
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            var response = await _elasticClient.IndexAsync(permission, idx => idx.Index(IndexName));

            if (!response.IsValidResponse)
            {
                _logger.LogError($"Error indexing permission: {response.DebugInformation}");
            }
        }

        public async Task<IReadOnlyCollection<Permission>> SearchPermissionsAsync(string query)
        {
            var response = await _elasticClient.SearchAsync<Permission>(s => s
                .Index(IndexName)
                .Query(q => q
                    .Wildcard(w => w
                        .Field(f => f.EmployeeName)
                        .Value($"*{query}*")
                    )
                )
            );

            if (!response.IsValidResponse)
            {
                _logger.LogError($"Error searching permissions: {response.DebugInformation}");
                return new List<Permission>();
            }

            return response.Documents;
        }
    }
}
