using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DOMTimeValueObject : ValueObject
    {
        public DOMTimeValueObject(DateTimeOffset pause, DateTimeOffset resume, DateTimeOffset difference)
        {
            Pause = pause;
            Resume = resume;
            Difference = difference;
        }
        
        public DateTimeOffset Pause { get; set; }
        public DateTimeOffset Resume { get; set; }
        public DateTimeOffset Difference { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Pause;
            yield return Resume;
            yield return Difference;
        }
    }
}
