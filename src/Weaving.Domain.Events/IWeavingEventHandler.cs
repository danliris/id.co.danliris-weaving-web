using Infrastructure.Domain.Events;

namespace Weaving.Domain.Events
{
    public interface IWeavingEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IWeavingEvent
    {
    }
}