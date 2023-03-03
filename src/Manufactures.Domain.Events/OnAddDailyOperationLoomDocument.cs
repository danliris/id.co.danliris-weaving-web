using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddDailyOperationLoomDocument : IManufactureEvent
    {
        public Guid Id { get; }

        public OnAddDailyOperationLoomDocument(Guid id)
        {
            Id = id;
        }
    }
}
