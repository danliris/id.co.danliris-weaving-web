using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DOMTimeValueObject : ValueObject
    {
        public DOMTimeValueObject(DateTimeOffset stop, DateTimeOffset remove, DateTimeOffset install, DateTimeOffset difference)
        {
            Stop = stop;
            Remove = remove;
            Install = install;
            Difference = difference;
        }

        public DateTimeOffset Stop { get; set; }
        public DateTimeOffset Remove { get; set; }
        public DateTimeOffset Install { get; set; }
        public DateTimeOffset Difference { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Stop;
            yield return Remove;
            yield return Install;
            yield return Difference;
        }
    }
}
