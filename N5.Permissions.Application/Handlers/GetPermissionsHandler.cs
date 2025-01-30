﻿// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/GetPermissionsHandler.cs

using MediatR;
using N5.Permissions.Application.Queries;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<Permission>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermissionsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Permission>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Permissions.GetAllAsync();
        }
    }
}
