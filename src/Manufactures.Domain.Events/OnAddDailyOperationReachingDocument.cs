using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddDailyOperationReachingDocument : IManufactureEvent
    {
        public Guid Id { get; }

        public OnAddDailyOperationReachingDocument(Guid id)
        {
            Id = id;
        }
    }
}
