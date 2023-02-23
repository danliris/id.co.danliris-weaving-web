using Manufactures.Domain.Shifts;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.Shift
{
    public class ShiftDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; }

        [JsonProperty(PropertyName = "StartTime")]
        public TimeSpan StartTime { get; }

        [JsonProperty(PropertyName = "EndTime")]
        public TimeSpan EndTime { get; }

        public ShiftDto(ShiftDocument document)
        {
            Id = document.Identity;
            Name = document.Name;
            StartTime =document.StartTime;
            EndTime = document.EndTime;
        }
    }
}
