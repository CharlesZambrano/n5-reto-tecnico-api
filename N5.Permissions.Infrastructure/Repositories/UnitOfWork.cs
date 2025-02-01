// *? n5-reto-tecnico-api/N5.Permissions.Infrastructure/Repositories/UnitOfWork.cs

using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Domain.Interfaces.Repositories;
using N5.Permissions.Infrastructure.Persistence;

namespace N5.Permissions.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IPermissionRepository Permissions { get; }
        public IPermissionTypeRepository PermissionTypes { get; }

        public UnitOfWork(ApplicationDbContext context,
                          IPermissionRepository permissionRepository,
                          IPermissionTypeRepository permissionTypeRepository)
        {
            _context = context;
            Permissions = permissionRepository;
            PermissionTypes = permissionTypeRepository;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
