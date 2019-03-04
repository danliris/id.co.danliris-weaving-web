using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Machines.ValueObjects
{
    public class MachineTypeValueObject : ValueObject
    {
        [JsonProperty(PropertyName="Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; }

        [JsonProperty(PropertyName = "SpeedStandard")]
        public int SpeedStandard { get; }

        [JsonProperty(PropertyName = "Unit")]
        public string Unit { get; }

        public MachineTypeValueObject(Guid id, string name, int speedStandard, string unit)
        {
            Id = id;
            Name = name;
            SpeedStandard = speedStandard;
            Unit = unit;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Name;
            yield return SpeedStandard;
            yield return Unit;
        }
    }
}
