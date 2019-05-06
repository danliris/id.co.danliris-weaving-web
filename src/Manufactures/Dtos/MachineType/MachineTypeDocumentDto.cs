using Manufactures.Domain.MachineTypes;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.MachineType
{
    public class MachineTypeDocumentDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "TypeName")]
        public string TypeName { get; }

        [JsonProperty(propertyName: "Speed")]
        public int Speed { get; }

        [JsonProperty(propertyName: "MachineUnit")]
        public string MachineUnit { get; }

        public MachineTypeDocumentDto(MachineTypeDocument document)
        {
            Id = document.Identity;
            TypeName = document.TypeName;
            Speed = document.Speed;
            MachineUnit = document.MachineUnit;
        }
    }
}
