using System;

namespace Manufactures.Domain.Events
{
    public class OnWeavingOrderPlaced : IManufactureEvent
    {
        public OnWeavingOrderPlaced(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}
