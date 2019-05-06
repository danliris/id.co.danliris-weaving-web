using Manufactures.Domain.Shifts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Shift
{
    public class ShiftDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; }

        [JsonProperty(PropertyName = "StartTime")]
        public string StartTime { get; }

        [JsonProperty(PropertyName = "EndTime")]
        public string EndTime { get; }

        public ShiftDto(ShiftDocument document)
        {
            Id = document.Identity;
            Name = document.Name;
            StartTime = document.StartTime;
            EndTime = document.EndTime;
        }
    }
}
