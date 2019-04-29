using System;

namespace Manufactures.Domain.Events
{
    public class OnAddBeam : IManufactureEvent
    {
        public Guid Id { get; }

        public OnAddBeam(Guid id)
        {
            Id = id;
        }
    }
}
