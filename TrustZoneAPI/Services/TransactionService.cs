using Microsoft.EntityFrameworkCore.Storage;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services
{
    public interface ITransactionService
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }
        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _repository.BeginTransactionAsync();
        }
    }
}
