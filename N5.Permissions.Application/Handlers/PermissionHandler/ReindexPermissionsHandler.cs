// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/ReindexPermissionsHandler.cs

using MediatR;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Infrastructure.Elasticsearch.Services;
using N5.Permissions.Application.Commands.PermissionCommand;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    // Handler para procesar el comando de reindexación de permisos
    public class ReindexPermissionsHandler : IRequestHandler<ReindexPermissionsCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ElasticsearchService _elasticsearchService;

        public ReindexPermissionsHandler(IUnitOfWork unitOfWork, ElasticsearchService elasticsearchService)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
        }

        public async Task<bool> Handle(ReindexPermissionsCommand request, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.Permissions.GetAllAsync();
            foreach (var permission in permissions)
            {
                await _elasticsearchService.IndexPermissionAsync(permission);
            }
            return true;
        }
    }
}
