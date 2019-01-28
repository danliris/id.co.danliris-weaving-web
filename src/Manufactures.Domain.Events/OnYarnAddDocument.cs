using System;

namespace Manufactures.Domain.Events
{
    public class OnYarnAddDocument : IManufactureEvent
    {
        public OnYarnAddDocument(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
