using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DOMTimeValueObject : ValueObject
    {
        public DOMTimeValueObject(DateTimeOffset replace, DateTimeOffset resume, DateTimeOffset difference)
        {
            Replace = replace;
            Resume = resume;
            Difference = difference;
        }
        
        public DateTimeOffset Replace { get; set; }
        public DateTimeOffset Resume { get; set; }
        public DateTimeOffset Difference { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Replace;
            yield return Resume;
            yield return Difference;
        }
    }
}
