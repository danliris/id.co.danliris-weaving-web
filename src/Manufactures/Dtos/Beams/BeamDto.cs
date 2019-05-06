using Manufactures.Domain.Beams;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Beams
{
    public class BeamDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; }

        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; }

        public BeamDto(BeamDocument document)
        {
            Id = document.Identity;
            Number = document.Number;
            Type = document.Type;
            EmptyWeight = document.EmptyWeight;
        }
    }
}
