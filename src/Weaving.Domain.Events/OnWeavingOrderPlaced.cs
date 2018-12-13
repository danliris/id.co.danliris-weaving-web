using System;

namespace Weaving.Domain.Events
{
    public class OnWeavingOrderPlaced : IWeavingEvent
    {
        public OnWeavingOrderPlaced(Guid orderId)
        {
            OrderID = orderId;
        }

        public Guid OrderID { get; }
    }
}
