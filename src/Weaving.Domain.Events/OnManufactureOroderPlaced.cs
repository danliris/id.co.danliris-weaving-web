using System;

namespace Weaving.Domain.Events
{
    public class OnManufactureOroderPlaced : IWeavingEvent
    {
        public OnManufactureOroderPlaced(Guid orderId)
        {
            this.OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}