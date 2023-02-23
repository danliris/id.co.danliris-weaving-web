using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddDailyOperationReachingHistory : IManufactureEvent
    {
        public Guid Id { get; }

        public OnAddDailyOperationReachingHistory(Guid id)
        {
            Id = id;
        }
    }
}
