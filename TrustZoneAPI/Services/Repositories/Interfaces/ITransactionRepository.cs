using Microsoft.EntityFrameworkCore.Storage;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
