// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/SearchPermissionsHandler.cs

using MediatR;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class SearchPermissionsHandler : IRequestHandler<SearchPermissionsQuery, IEnumerable<Permission>>
    {
        private readonly ElasticsearchService _elasticsearchService;

        public SearchPermissionsHandler(ElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        public async Task<IEnumerable<Permission>> Handle(SearchPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _elasticsearchService.SearchPermissionsAsync(request.Query);
        }
    }
}
