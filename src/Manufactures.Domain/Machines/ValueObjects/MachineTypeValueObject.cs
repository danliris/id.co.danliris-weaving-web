using Manufactures.Domain.MachineTypes;
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

        [JsonProperty(PropertyName = "TypeName")]
        public string TypeName { get; }

        [JsonProperty(PropertyName = "Speed")]
        public int Speed { get; }

        [JsonProperty(PropertyName = "MachineUnit")]
        public string MachineUnit { get; }

        public MachineTypeValueObject(MachineTypeDocument machineType)
        {
            Id = machineType.Identity;
            TypeName = machineType.TypeName;
            Speed = machineType.Speed;
            MachineUnit = machineType.MachineUnit;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return TypeName;
            yield return Speed;
            yield return MachineUnit;
        }
    }
}
