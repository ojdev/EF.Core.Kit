using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.Kit.SeedWork
{
    public class DefaultRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : TEntity<TKey>
        where TKey : IComparable, IComparable<TKey>
    {

        private readonly KitDbContext _context;
        protected DbSet<TEntity> Table => _context.Set<TEntity>();

        public DefaultRepository(KitDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveEntitiesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var entity = await Table.FindAsync(key, cancellationToken);
            if (entity != null)
            {
                _context.Remove(entity);
                return await _context.SaveEntitiesAsync(cancellationToken);
            }
            return false;
        }

        public async Task<bool> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entities = Table.Where(predicate);
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            await _context.SaveEntitiesAsync(cancellationToken);
            return true;
        }

        public async Task<TEntity> GetAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken)) => await Table.FindAsync(keyValues: new object[] { key }, cancellationToken: cancellationToken);

        public async Task<TEntity> MergeAsync(TKey key, Action<MegreProperty<TEntity, TKey>> megrePropertyAction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var old = await _context.FindAsync<TEntity>(key);
            var megreOption = new MegreProperty<TEntity, TKey>(_context.Entry(old));
            megrePropertyAction(megreOption);
            await _context.SaveEntitiesAsync(cancellationToken);
            return old;
        }

        public IQueryable<TEntity> Query() => Table.AsNoTracking();

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var old = _context.Find<TEntity>(entity.Id);
            _context.Entry(old).CurrentValues.SetValues(entity);
            await _context.SaveEntitiesAsync(cancellationToken);
            return entity;
        }
    }
}
