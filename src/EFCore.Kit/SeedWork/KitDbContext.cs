using EFCore.Kit.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.Kit.SeedWork
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class KitDbContext : DbContext
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mediator"></param>
        public KitDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entry in ChangeTracker.Entries().Where(t => t.State == EntityState.Deleted || t.State == EntityState.Modified))
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        {
                            entry.CurrentValues["DeletionTime"] = DateTimeOffset.Now;
                            entry.CurrentValues["IsDeleted"] = true;
                            entry.State = EntityState.Modified;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            entry.CurrentValues["LastUpdateTime"] = DateTimeOffset.Now;
                            break;
                        }
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            bool saveResult = false;
            try
            {
                int result = await base.SaveChangesAsync();
                saveResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (saveResult)
                {
                    await _mediator.DispatchDomainEventsAsync(this);
                }
            }
            return saveResult;
        }
    }
}
