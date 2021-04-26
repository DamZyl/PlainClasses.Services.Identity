using System.Threading;
using System.Threading.Tasks;
using PlainClasses.Services.Identity.Domain.SharedKernels;

namespace PlainClasses.Services.Identity.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot;

        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        void Rollback();
    }
}