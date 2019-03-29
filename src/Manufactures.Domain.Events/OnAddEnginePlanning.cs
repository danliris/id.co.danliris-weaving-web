using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnAddEnginePlanning : IManufactureEvent
    {
        public Guid Id { get; }
        
        public OnAddEnginePlanning(Guid id)
        {
            Id = id;
        }

    }
}
