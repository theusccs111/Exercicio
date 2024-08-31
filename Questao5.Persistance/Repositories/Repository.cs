using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Questao5.Application.Interfaces.Persistance.Repositories;
using Questao5.Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Persistance.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private readonly Questao5Context _context;
        private DbSet<TEntity> set;
        private bool disposed = false;

        public Repository(Questao5Context context)
        {
            _context = context;
        }

        public virtual IQueryable<TEntity> Entities
        {
            get { return Set; }
        }

        protected DbSet<TEntity> Set
        {
            get { return set ??= _context.Set<TEntity>(); }
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public IDbContextTransaction CurrentTransaction()
        {
            return _context.Database.CurrentTransaction;
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await Set?.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Set.ToListAsync(cancellationToken);
        }

        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(null, orderBy, includeProperties, skip, take);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return await GetQueryable(null, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(filter, orderBy, includeProperties, skip, take);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return GetQueryable(filter, null, includeProperties).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            return await GetQueryable(filter, null, includeProperties).FirstOrDefaultAsync();
        }

        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            return GetQueryable(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        public virtual async Task<decimal> GetMaxAsync(Expression<Func<TEntity, decimal>> filter)
        {
            return await Set.MaxAsync(filter, default);
        }

        public virtual decimal GetMax(Expression<Func<TEntity, decimal>> filter)
        {
            return Set.Max(filter);
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Set.CountAsync(filter, default);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> filter)
        {
            return Set.Count(filter);
        }

        public virtual bool GetExists(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Any();
        }

        public virtual Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).AnyAsync();
        }

        public virtual void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public virtual TEntity AddEntity(TEntity entity)
        {
            return Set.Add(entity).Entity;
        }

        public virtual void Update(TEntity entity)
        {
            Set.Update(entity);
        }

        public virtual void DetachEntry(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual void DetachEntryRange(IEnumerable<TEntity> entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual void ModifyEntry(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(TEntity entity)
        {
            Set.Remove(entity);

        }
        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            Set.RemoveRange(entity);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
        //public async virtual Task<long> SaveAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}
        public virtual TEntity Find(Expression<Func<TEntity, bool>> match)
        {
            return Set.AsNoTracking().FirstOrDefault(match);
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
        {
            return Set.Where(match);
        }
        public IEnumerable<TEntity> FromSqlRaw(string query)
        {
            return Set.FromSqlRaw(query).ToList();
        }
        public async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            return await Set.Where(match).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllIEnumerableAsync(Expression<Func<TEntity, bool>> match)
        {
            return await Set.Where(match).ToListAsync();
        }

        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            includeProperties ??= string.Empty;
            IQueryable<TEntity> query = Set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public DbSet<TEntity> DbSet()
        {
            return Set;
        }

        public IQueryable<TEntity> ToQueryable()
        {
            return Set.AsQueryable();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposed = true;
            }
        }
    }
}
