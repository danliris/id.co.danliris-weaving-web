using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IManufactureEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IManufactureEvent
    {
    }
}