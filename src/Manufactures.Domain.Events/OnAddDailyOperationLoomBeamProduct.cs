using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddDailyOperationLoomBeamProduct : IManufactureEvent
    {
        public Guid Id { get; }

        public OnAddDailyOperationLoomBeamProduct(Guid id)
        {
            Id = id;
        }
    }
}
