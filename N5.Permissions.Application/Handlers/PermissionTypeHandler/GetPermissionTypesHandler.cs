// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionTypeHandler/GetPermissionTypesHandler.cs

using MediatR;
using AutoMapper;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Queries.PermissionTypeQuerie;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers.PermissionTypeHandler
{
    public class GetPermissionTypesHandler : IRequestHandler<GetPermissionTypesQuery, IEnumerable<PermissionTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPermissionTypesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionTypeDto>> Handle(GetPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            var permissionTypes = await _unitOfWork.PermissionTypes.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionTypeDto>>(permissionTypes);
        }
    }
}
