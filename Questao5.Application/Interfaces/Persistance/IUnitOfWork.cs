using Microsoft.EntityFrameworkCore.ChangeTracking;
using Questao5.Application.Interfaces.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Interfaces.Persistance
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void DetachAllEntities();
        IEnumerable<EntityEntry> CheckEntryAtached();
        void EntityStateRemove<T>(T entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func);
    }
}
