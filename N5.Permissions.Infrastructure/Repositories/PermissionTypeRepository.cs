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
            return await _context.PermissionTypes.ToListAsync();
        }

        public async Task<PermissionType?> GetByIdAsync(int id)
        {
            return await _context.PermissionTypes.FindAsync(id);
        }
    }
}
