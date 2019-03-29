using Manufactures.Domain.MachineTypes;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class MachineType : ValueObject
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "TypeName")]
        public string TypeName { get; }

        [JsonProperty(propertyName: "Speed")]
        public int Speed { get; }

        [JsonProperty(propertyName: "MachineUnit")]
        public string MachineUnit { get; }

        public MachineType (MachineTypeDocument document)
        {
            Id = document.Identity;
            TypeName = document.TypeName;
            Speed = document.Speed;
            MachineUnit = document.MachineUnit;
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
