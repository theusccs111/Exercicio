using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Interfaces.Persistance.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Entities
        {
            get;
        }
        IDbContextTransaction BeginTransaction();
        IDbContextTransaction CurrentTransaction();
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> FromSqlRaw(string query);
        int GetCount(Expression<Func<TEntity, bool>> filter);
        decimal GetMax(Expression<Func<TEntity, decimal>> filter);
        Task<decimal> GetMaxAsync(Expression<Func<TEntity, decimal>> filter);
        Task<List<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);

        TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null);

        TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null);

        bool GetExists(Expression<Func<TEntity, bool>> filter = null);
        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null);
        void Add(TEntity entity);
        TEntity AddEntity(TEntity entity);
        void Update(TEntity entity);
        void DetachEntry(TEntity entity);
        void DetachEntryRange(IEnumerable<TEntity> entity);
        void ModifyEntry(TEntity entity);
        void Remove(TEntity entity);
        void Save();
        //Task<long> SaveAsync();
        TEntity Find(Expression<Func<TEntity, bool>> match);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> match);

        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match);

        Task<IEnumerable<TEntity>> FindAllIEnumerableAsync(Expression<Func<TEntity, bool>> match);

        IQueryable<TEntity> GetQueryable(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    string includeProperties = null,
                    int? skip = null,
                    int? take = null);

        void Dispose();

        DbSet<TEntity> DbSet();
        IQueryable<TEntity> ToQueryable();
    }
}
