// *? n5-reto-tecnico-api/N5.Permissions.Infrastructure/Repositories/PermissionTypeRepository.cs

using Microsoft.EntityFrameworkCore;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces.Repositories;
using N5.Permissions.Infrastructure.Persistence;

namespace N5.Permissions.Infrastructure.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermissionType>> GetAllAsync()
        {
            return await _context.PermissionTypes
                .Include(p => p.Permissions)
                .ToListAsync();
        }

        public async Task<PermissionType?> GetByIdAsync(int id)
        {
            return await _context.PermissionTypes
                .Include(p => p.Permissions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsByCode(string code)
        {
            return await _context.PermissionTypes.AnyAsync(pt => pt.Code == code);
        }

        public async Task AddAsync(PermissionType permissionType)
        {
            await _context.PermissionTypes.AddAsync(permissionType);
        }
    }
}
