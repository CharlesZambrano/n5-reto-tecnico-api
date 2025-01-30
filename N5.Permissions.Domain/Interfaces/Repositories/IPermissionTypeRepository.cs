// *? n5-reto-tecnico-api/N5.Permissions.Domain/Interfaces/Repositories/IPermissionTypeRepository.cs

using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Domain.Interfaces.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<IEnumerable<PermissionType>> GetAllAsync();
        Task<PermissionType?> GetByIdAsync(int id);
        Task AddAsync(PermissionType permissionType);
    }
}
