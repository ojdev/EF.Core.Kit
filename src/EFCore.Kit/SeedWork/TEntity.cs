using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Kit.SeedWork
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class TEntity<TKey> : IEntity where TKey : IComparable, IComparable<TKey>
    {
        int? _requestedHashCode;
        TKey _Id;
        /// <summary>
        /// 
        /// </summary>
        public virtual TKey Id
        {
            get => _Id;
            protected set => _Id = value;
        }
        /// <summary>
        /// 租户(CityId)
        /// </summary>
        public string TenantId { get; set; }
        private DateTimeOffset _creationTime;
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public DateTimeOffset CreationTime => _creationTime;
        private DateTimeOffset? _lastUpdateTime;
        private List<INotification> _domainEvents;
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventItem"></param>
        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }
        /// <summary>
        /// 
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default(TKey));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TEntity<TKey>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            TEntity<TKey> item = (TEntity<TKey>)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return EqualityComparer<TKey>.Default.Equals(item.Id, Id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TEntity<TKey> left, TEntity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TEntity<TKey> left, TEntity<TKey> right)
        {
            return !(left == right);
        }
    }
}
