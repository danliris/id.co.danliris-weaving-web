using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.ValueObjects
{
    public class DailyOperationReachingValueObject : ValueObject
    {
        public string ReachingTypeInput { get; set; }

        public string ReachingTypeOutput { get; set; }

        public double ReachingWidth { get; set; }

        public DailyOperationReachingValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ReachingTypeInput;
            yield return ReachingTypeOutput;
            yield return ReachingWidth;
        }
    }
}
