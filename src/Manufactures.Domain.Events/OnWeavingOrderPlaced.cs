using System;
using System.Collections.Generic;
using System.Text;

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
