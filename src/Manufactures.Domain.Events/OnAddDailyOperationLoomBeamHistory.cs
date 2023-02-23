using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddDailyOperationLoomBeamHistory : IManufactureEvent
    {
        public Guid Id { get; }

        public OnAddDailyOperationLoomBeamHistory(Guid id)
        {
            Id = id;
        }
    }
}
