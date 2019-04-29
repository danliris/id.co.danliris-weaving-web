using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddDailyOperationLoom : IManufactureEvent
    {
        public Guid Id { get; }

        public OnAddDailyOperationLoom(Guid id)
        {
            Id = id;
        }
    }
}
