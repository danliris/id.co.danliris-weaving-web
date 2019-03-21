using Manufactures.Domain.MachineTypes;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.MachineType
{
    public class MachineTypeListDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "TypeName")]
        public string TypeName { get; }

        public MachineTypeListDto(MachineTypeDocument document)
        {
            Id = document.Identity;
            TypeName = document.TypeName;
        }
    }
}
