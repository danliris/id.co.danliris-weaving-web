using Manufactures.Domain.Machines;
using Manufactures.Domain.MachineTypes;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class ManufactureMachine : ValueObject
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(propertyName: "Location")]
        public string Location { get; }

        [JsonProperty(propertyName: "MachineType")]
        public MachineType MachineType { get; }

        [JsonProperty(propertyName: "WeavingUnitId")]
        public UnitId WeavingUnitId { get; }

        public ManufactureMachine(MachineDocument machineDocument,
                       MachineTypeDocument machineTypeDocument)
        {
            Id = machineDocument.Identity;
            MachineNumber = machineDocument.MachineNumber;
            Location = machineDocument.Location;
            WeavingUnitId = machineDocument.WeavingUnitId;
            MachineType = new MachineType(machineTypeDocument);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return MachineNumber;
            yield return Location;
            yield return MachineType;
            yield return WeavingUnitId;
        }
    }
}
