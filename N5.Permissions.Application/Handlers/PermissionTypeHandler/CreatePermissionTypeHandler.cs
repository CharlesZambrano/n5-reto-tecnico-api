// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionTypeHandler/CreatePermissionTypeHandler.cs

using MediatR;
using AutoMapper;
using N5.Permissions.Application.Commands.PermissionTypeCommand;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace N5.Permissions.Application.Handlers.PermissionTypeHandler
{
    public class CreatePermissionTypeHandler : IRequestHandler<CreatePermissionTypeCommand, PermissionTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePermissionTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PermissionTypeDto> Handle(CreatePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
                throw new ValidationException("Description is required.");
            if (string.IsNullOrWhiteSpace(request.Code))
                throw new ValidationException("Code is required.");
            if (request.Code.Length != 3)
                throw new ValidationException("Code must be exactly 3 characters long.");

            if (await _unitOfWork.PermissionTypes.ExistsByCode(request.Code))
                throw new ArgumentException("Permission type with this code already exists.");

            var permissionType = new PermissionType
            {
                Description = request.Description,
                Code = request.Code,
                Permissions = new List<Permission>()
            };

            await _unitOfWork.PermissionTypes.AddAsync(permissionType);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<PermissionTypeDto>(permissionType);
        }
    }
}
