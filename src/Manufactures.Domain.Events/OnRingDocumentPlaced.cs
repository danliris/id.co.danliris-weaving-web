using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnRingDocumentPlaced : IManufactureEvent
    {
        public OnRingDocumentPlaced(Guid identity)
        {
            Identity = identity;
        }

        public Guid Identity { get; }
    }
}
