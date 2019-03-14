using System;

namespace Manufactures.Domain.Events
{
    public class OnYarnNumberDocumentPlaced : IManufactureEvent
    {
        public OnYarnNumberDocumentPlaced(Guid identity)
        {
            Identity = identity;
        }

        public Guid Identity { get; }
    }
}
