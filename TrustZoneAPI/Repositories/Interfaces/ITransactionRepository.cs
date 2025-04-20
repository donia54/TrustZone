using Microsoft.EntityFrameworkCore.Storage;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
