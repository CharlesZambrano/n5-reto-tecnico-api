// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/SearchPermissionsHandler.cs

using MediatR;
using AutoMapper;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class SearchPermissionsHandler : IRequestHandler<SearchPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly ElasticsearchService _elasticsearchService;
        private readonly IMapper _mapper;

        public SearchPermissionsHandler(ElasticsearchService elasticsearchService, IMapper mapper)
        {
            _elasticsearchService = elasticsearchService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(SearchPermissionsQuery request, CancellationToken cancellationToken)
        {
            // Buscar en Elasticsearch los documentos
            var esDocs = await _elasticsearchService.SearchPermissionsAsync(request.Query);

            // Mapear al DTO de salida
            return _mapper.Map<IEnumerable<PermissionDto>>(esDocs);
        }
    }
}
