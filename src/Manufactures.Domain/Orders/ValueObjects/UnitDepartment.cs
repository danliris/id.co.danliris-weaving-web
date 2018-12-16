using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class UnitDepartment : ValueObject
    {
        public UnitDepartment(Guid departmentId, string name)
        {
            Identity = departmentId;
            Name = name;
        }

        public Guid Identity { get; }

        public string Name { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}