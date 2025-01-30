// *? n5-reto-tecnico-api/N5.Permissions.Domain/Interfaces/IUnitOfWork.cs

using N5.Permissions.Domain.Interfaces.Repositories;

namespace N5.Permissions.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository Permissions { get; }
        IPermissionTypeRepository PermissionTypes { get; }
        Task<int> CommitAsync();
    }
}
