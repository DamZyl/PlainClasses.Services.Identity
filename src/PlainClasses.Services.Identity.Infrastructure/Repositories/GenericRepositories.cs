using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PlainClasses.Services.Identity.Domain.Repositories;
using PlainClasses.Services.Identity.Domain.SharedKernels;
using PlainClasses.Services.Identity.Infrastructure.Databases;

namespace PlainClasses.Services.Identity.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        private readonly IdentityContext _context;

        public GenericRepository(IdentityContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
            => await _context.Set<TEntity>().AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
            => await _context.Set<TEntity>().AddRangeAsync(entities);

        public void Edit(TEntity entity)
            => _context.Set<TEntity>().Update(entity);

        public async Task EditAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await Task.CompletedTask;
        }

        public void EditRange(IEnumerable<TEntity> entities)
            => _context.Set<TEntity>().UpdateRange(entities);

        public async Task EditRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            await Task.CompletedTask;
        }

        public void Delete(TEntity entity) 
            => _context.Set<TEntity>().Remove(entity);
        

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await Task.CompletedTask;
        }

        public void DeleteRange(IEnumerable<TEntity> entities) 
            => _context.Set<TEntity>().RemoveRange(entities);

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
            => await _context.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
            => await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
            => await _context.Set<TEntity>().Where(predicate).ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity> FindByWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();

            if (includes != null)
            {
                queryable = includes(queryable);
            }

            return await queryable.SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAllWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();

            if (includes != null)
            {
                queryable = includes(queryable);
            }

            return await queryable.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();

            if (includes != null)
            {
                queryable = includes(queryable);
            }

            return await queryable.ToListAsync();
        }
    }
}