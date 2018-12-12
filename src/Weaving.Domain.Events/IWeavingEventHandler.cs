using ExtCore.Events;

namespace Weaving.Domain.Events
{
    public interface IWeavingEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IWeavingEvent
    {
    }
}