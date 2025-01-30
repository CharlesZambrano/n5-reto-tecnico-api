using MediatR;
using N5.Permissions.Application.Queries;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Infrastructure.Elasticsearch.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N5.Permissions.Application.Handlers
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
