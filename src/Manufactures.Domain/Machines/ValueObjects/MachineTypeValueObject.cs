using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Machines.ValueObjects
{
    public class MachineTypeValueObject : ValueObject
    {
        public string Name { get; }
        public int Rpm { get; }
        public string Unit { get; }

        public MachineTypeValueObject(string name, int rpm, string unit)
        {
            Name = name;
            Rpm = rpm;
            Unit = unit;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Rpm;
            yield return Unit;
        }
    }
}
