using N5.Permissions.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N5.Permissions.Domain.Interfaces.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<IEnumerable<PermissionType>> GetAllAsync();
        Task<PermissionType?> GetByIdAsync(int id);
    }
}
