using System;

namespace Manufactures.Domain.Events
{
    public class OnManufactureOrderPlaced : IManufactureEvent
    {
        public OnManufactureOrderPlaced(Guid orderId)
        {
            OrderID = orderId;
        }

        public Guid OrderID { get; }
    }
}