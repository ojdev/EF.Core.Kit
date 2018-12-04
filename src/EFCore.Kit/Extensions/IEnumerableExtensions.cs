using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore.Kit.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> WhereIf<TEntity>(this IEnumerable<TEntity> source, bool condition, Func<TEntity, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
