using EFCore.Kit.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <param name="optionsLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddEFCoreKitDbContext<TContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : RDbContext
        {
            serviceCollection.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);
            switch (contextLifetime)
            {
                case ServiceLifetime.Scoped:
                    {
                        serviceCollection.TryAddScoped<RDbContext, TContext>();
                        serviceCollection.TryAddScoped(typeof(IRepository<>), typeof(DefaultRepository<>));
                        serviceCollection.TryAddScoped(typeof(IRepository<,>), typeof(DefaultRepository<,>));
                        break;
                    }
                case ServiceLifetime.Singleton:
                    {
                        serviceCollection.TryAddSingleton<RDbContext, TContext>();
                        serviceCollection.TryAddSingleton(typeof(IRepository<>), typeof(DefaultRepository<>));
                        serviceCollection.TryAddSingleton(typeof(IRepository<,>), typeof(DefaultRepository<,>));
                        break;
                    }
                case ServiceLifetime.Transient:
                    {
                        serviceCollection.TryAddTransient<RDbContext, TContext>();
                        serviceCollection.TryAddTransient(typeof(IRepository<>), typeof(DefaultRepository<>));
                        serviceCollection.TryAddTransient(typeof(IRepository<,>), typeof(DefaultRepository<,>));
                        break;
                    }
            }
            return serviceCollection;
        }
    }
}
