using ExtCore.Events;
using System.Threading.Tasks;

namespace Weaving.Domain.Events
{
    public static class EventExtensions 
    {
        public static void Broadcast<TEvent>(this TEvent @event) where TEvent : IWeavingEvent
        {
            Task.Factory.StartNew(() =>
            {
                Event<IWeavingEventHandler<TEvent>, TEvent>.Broadcast(@event);
            });
        }
    }
}
