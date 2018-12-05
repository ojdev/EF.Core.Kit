using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq.Expressions;

namespace EFCore.Kit.SeedWork
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class MegreProperty<TEntity,TKey>
        where TEntity : Entity<TKey>
        where TKey : IComparable, IComparable<TKey>
    {
        private EntityEntry<TEntity> _entity;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public MegreProperty(EntityEntry<TEntity> entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="value"></param>
        public void MegreValue<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, TProperty value)
        {
            var propertyEntry = _entity.Property(propertyExpression);
            if (value?.Equals(propertyEntry.OriginalValue) == false)
            {
                propertyEntry.CurrentValue = value;
                propertyEntry.IsModified = true;
            }
        }
    }
}
