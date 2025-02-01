// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/SearchPermissionsHandler.cs

using MediatR;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class SearchPermissionsHandler : IRequestHandler<SearchPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly ElasticsearchService _elasticsearchService;

        public SearchPermissionsHandler(ElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(SearchPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _elasticsearchService.SearchPermissionsAsync(request.Query);

            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                EmployeeName = p.EmployeeName,
                EmployeeSurname = p.EmployeeSurname,
                PermissionTypeId = p.PermissionTypeId,
                PermissionDate = p.PermissionDate,
                PermissionType = new PermissionTypeDto
                {
                    Id = p.PermissionType.Id,
                    Description = p.PermissionType.Description,
                    Code = p.PermissionType.Code
                }
            }).ToList();
        }
    }
}
