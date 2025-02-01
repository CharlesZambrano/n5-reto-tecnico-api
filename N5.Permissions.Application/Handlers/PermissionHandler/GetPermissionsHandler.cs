// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/GetPermissionsHandler.cs

using MediatR;
using AutoMapper;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly ElasticsearchService _elasticsearchService;
        private readonly IMapper _mapper;

        public GetPermissionsHandler(ElasticsearchService elasticsearchService, IMapper mapper)
        {
            _elasticsearchService = elasticsearchService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            // Se consultan todos los documentos de ES
            var esDocs = await _elasticsearchService.GetAllPermissionsAsync();

            // Mapear de EsPermissionDoc -> PermissionDto (usando EsPermissionDocProfile)
            return _mapper.Map<IEnumerable<PermissionDto>>(esDocs);
        }
    }
}
