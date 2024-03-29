﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnFabricConstructionPlaced : IManufactureEvent
    {
        public OnFabricConstructionPlaced(Guid identity)
        {
            Identity = identity;
        }

        public Guid Identity { get; }
    }
}
