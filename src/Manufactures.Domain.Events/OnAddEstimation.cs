using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddEstimation : IManufactureEvent
    {
        public Guid Id { get; }


        public OnAddEstimation(Guid id)
        {
            Id = id;
        }
    }
}
