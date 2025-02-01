// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/CreatePermissionHandler.cs

using MediatR;
using AutoMapper;
using N5.Permissions.Application.Commands.PermissionCommand;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ElasticsearchService _elasticsearchService;
        private readonly IMapper _mapper;

        public CreatePermissionHandler(IUnitOfWork unitOfWork, ElasticsearchService elasticsearchService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
            _mapper = mapper;
        }

        public async Task<PermissionDto> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.PermissionTypeId);
            if (permissionType == null)
                throw new ArgumentException("Invalid PermissionType ID");

            var permission = new Permission
            {
                EmployeeName = request.EmployeeName,
                EmployeeSurname = request.EmployeeSurname,
                PermissionTypeId = request.PermissionTypeId,
                PermissionType = permissionType,
                PermissionDate = request.PermissionDate
            };

            await _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.CommitAsync();

            await _elasticsearchService.IndexPermissionAsync(permission);

            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
