using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlainClasses.Services.Identity.Domain.Repositories;
using PlainClasses.Services.Identity.Domain.SharedKernels;
using PlainClasses.Services.Identity.Infrastructure.Databases;

namespace PlainClasses.Services.Identity.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        
        public UnitOfWork(IdentityContext context)
        {
            _context = context;
        }
        
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IGenericRepository<TEntity>;
            }

            IGenericRepository<TEntity> repository = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }
    }
}