using Equatorial.MPE.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Questao5.Application.Interfaces.Persistance;
using Questao5.Application.Interfaces.Persistance.Repositories;
using Questao5.Persistance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Questao5Context _context;

        private Hashtable repositories;

        public UnitOfWork(DbContextOptions<Questao5Context> options)
        {
            _context = new Questao5Context(options);
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            finally
            {
                repositories = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            finally
            {
                repositories = null;
            }
        }

        public async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                repositories = null;
            }
        }

        public async Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            var transResult = await strategy.ExecuteAsync(async () =>
            {
                using (var trans = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var result = await func.Invoke();
                        await trans.CommitAsync();
                        return result;
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                        throw;
                    }
                }
            });
            return transResult;
        }
        public IRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Hashtable();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                Type repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)repositories[type];
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public IEnumerable<EntityEntry> CheckEntryAtached()
        {
            return _context.ChangeTracker.Entries().ToList();
        }

        public void EntityStateRemove<T>(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
