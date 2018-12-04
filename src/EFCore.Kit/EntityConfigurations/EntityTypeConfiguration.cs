using EFCore.Kit.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EFCore.Kit.EntityConfigurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class EntityTypeConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : TEntity<TKey>
        where TKey : IComparable, IComparable<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Ignore(o => o.DomainEvents);
            builder.Property<DateTimeOffset>("CreationTime").HasField("_creationTime").HasDefaultValue(DateTimeOffset.Now).ValueGeneratedOnAdd();
            builder.Property<DateTimeOffset?>("LastUpdateTime").HasField("_lastUpdateTime");
            builder.Property<DateTimeOffset?>("DeletionTime");
            builder.Property<bool?>("IsDeleted").HasDefaultValue(false);
            builder.Property<byte[]>("RowVersion").IsRowVersion();
            builder.HasQueryFilter(o => EF.Property<bool>(o, "IsDeleted") == false);
        }
    }
}
