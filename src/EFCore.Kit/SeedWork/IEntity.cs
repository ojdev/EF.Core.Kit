using MediatR;
using System.Collections.Generic;

namespace EFCore.Kit.SeedWork
{
    public interface IEntity
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }

        void AddDomainEvent(INotification eventItem);
        void ClearDomainEvents();
        void RemoveDomainEvent(INotification eventItem);
    }
}
