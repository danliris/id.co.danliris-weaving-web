using Manufactures.Domain.Shifts;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Shift
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
            Name = document.Name;
            StartTime = DateTimeOffset.Parse(document.StartTime).TimeOfDay;
            EndTime = DateTimeOffset.Parse(document.EndTime).TimeOfDay;
        }
    }
}
