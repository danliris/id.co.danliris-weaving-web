using Manufactures.Domain.DailyOperations.Commands;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DOMTimeValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Pause")]
        public DateTimeOffset Pause { get; set; }

        [JsonProperty(PropertyName = "Resume")]
        public DateTimeOffset Resume { get; set; }

        [JsonProperty(PropertyName = "Difference")]
        public int Difference { get; set; }

        public DOMTimeValueObject(DateTimeOffset pause, 
                                  DateTimeOffset resume, 
                                  DateTimeOffset difference)
        {
            Pause = pause;
            Resume = resume;
            Difference = pause.Subtract(resume).Hours;
        }

        public DOMTimeValueObject(DailyOperationMachineTimeCommand dailyOperationMachine)
        {
            Pause = dailyOperationMachine.Pause;
            Resume = dailyOperationMachine.Resume;
            Difference = 
                dailyOperationMachine.Pause
                                    .Subtract(dailyOperationMachine.Resume)
                                    .Hours;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Pause;
            yield return Resume;
            yield return Difference;
        }
    }
}
