using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq.Expressions;

namespace EFCore.Kit.SeedWork
{
    public class MegreProperty<TEntity,TKey>
        where TEntity : TEntity<TKey>
        where TKey : IComparable, IComparable<TKey>
    {
        private EntityEntry<TEntity> _entity;

        public MegreProperty(EntityEntry<TEntity> entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

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
