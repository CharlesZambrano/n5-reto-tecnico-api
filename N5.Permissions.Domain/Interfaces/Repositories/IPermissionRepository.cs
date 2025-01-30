// *? n5-reto-tecnico-api/N5.Permissions.Domain/Interfaces/Repositories/IPermissionRepository.cs

using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Domain.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(int id);
        Task AddAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(int id);
    }
}
