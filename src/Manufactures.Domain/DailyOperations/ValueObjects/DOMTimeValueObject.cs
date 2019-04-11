using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DOMTimeValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Pause")]
        public DateTimeOffset Pause { get; set; }

        [JsonProperty(PropertyName = "Resume")]
        public DateTimeOffset Resume { get; set; }

        [JsonProperty(PropertyName = "Difference")]
        public DateTimeOffset Difference { get; set; }

        public DOMTimeValueObject(DateTimeOffset pause, DateTimeOffset resume, DateTimeOffset difference)
        {
            Pause = pause;
            Resume = resume;
            Difference = difference;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Pause;
            yield return Resume;
            yield return Difference;
        }
    }
}
