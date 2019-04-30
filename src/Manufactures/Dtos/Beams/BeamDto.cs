using Manufactures.Domain.Beams;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Beams
{
    public class BeamDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "BeamType")]
        public string BeamType { get; }

        public BeamDto(BeamDocument document)
        {
            Id = document.Identity;
            BeamNumber = document.BeamNumber;
            BeamType = document.BeamType;
        }
    }
}
