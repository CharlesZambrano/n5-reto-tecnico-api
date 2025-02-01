// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/GetPermissionByIdHandler.cs

using MediatR;
using AutoMapper;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class GetPermissionByIdHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPermissionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PermissionDto?> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
            if (permission == null)
                return null;
            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
