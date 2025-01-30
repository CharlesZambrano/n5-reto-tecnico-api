using MediatR;
using N5.Permissions.Application.Queries;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N5.Permissions.Application.Handlers
{
    public class GetPermissionTypesHandler : IRequestHandler<GetPermissionTypesQuery, IEnumerable<PermissionType>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermissionTypesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PermissionType>> Handle(GetPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PermissionTypes.GetAllAsync();
        }
    }
}
