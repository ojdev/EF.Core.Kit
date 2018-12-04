using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Kit.SeedWork
{
    public abstract class TEntity<TKey> : IEntity where TKey : IComparable, IComparable<TKey>
    {
        int? _requestedHashCode;
        TKey _Id;
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
        [NotMapped]
        public DateTimeOffset CreationTime => _creationTime;
        private DateTimeOffset? _lastUpdateTime;
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default(TKey));
        }

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
        public static bool operator ==(TEntity<TKey> left, TEntity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(TEntity<TKey> left, TEntity<TKey> right)
        {
            return !(left == right);
        }
    }
}
