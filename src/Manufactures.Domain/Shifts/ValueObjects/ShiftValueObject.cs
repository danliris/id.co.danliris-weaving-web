using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shifts.ValueObjects
{
    public class ShiftValueObject : ValueObject
    {
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public ShiftValueObject(string name, TimeSpan startTime, TimeSpan endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }

        public ShiftValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return StartTime;
            yield return EndTime;
        }
    }
}
