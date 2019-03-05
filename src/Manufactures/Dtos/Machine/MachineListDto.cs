using Manufactures.Domain.Machines;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Machine
{
    public class MachineListDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "Location")]
        public string Location { get; }

        public MachineListDto(MachineDocument document)
        {
            Id = document.Identity;
            Location = document.Location;
        }
    }
}
