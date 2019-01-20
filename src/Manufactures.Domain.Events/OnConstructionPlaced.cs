using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnConstructionPlaced : IManufactureEvent
    {
        public OnConstructionPlaced(Guid identity)
        {
            Identity = identity;
        }

        public Guid Identity { get; }
    }
}
