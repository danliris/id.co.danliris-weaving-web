using System;

namespace Manufactures.Domain.Events
{
    public class OnMaterialTypePlace : IManufactureEvent
    {
        public OnMaterialTypePlace(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
