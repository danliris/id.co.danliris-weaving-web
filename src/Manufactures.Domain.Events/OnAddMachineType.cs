using System;

namespace Manufactures.Domain.Events
{
    public class OnAddMachineType : IManufactureEvent
    {
        public Guid Id { get; }


        public OnAddMachineType(Guid id)
        {
            Id = id;
        }
    }
}
