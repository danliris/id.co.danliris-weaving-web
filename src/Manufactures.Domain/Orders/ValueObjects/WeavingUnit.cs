using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class WeavingUnit : ValueObject
    {
        public WeavingUnit(Guid unitId, string name)
        {
            UnitId = unitId;
            Name = name;
        }

        public Guid UnitId { get; private set; }
        public string Name { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
