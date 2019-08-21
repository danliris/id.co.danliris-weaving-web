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

        public DailyOperationReachingValueObject(string reachingTypeInput, string reachingTypeOutput, double reachingWidth)
        {
            ReachingTypeInput = reachingTypeInput;
            ReachingTypeOutput = reachingTypeOutput;
            ReachingWidth = reachingWidth;
        }

        public DailyOperationReachingValueObject(string reachingTypeInput, string reachingTypeOutput)
        {
            ReachingTypeInput = reachingTypeInput;
            ReachingTypeOutput = reachingTypeOutput;
        }

        public DailyOperationReachingValueObject(double reachingWidth)
        {
            ReachingWidth = reachingWidth;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ReachingTypeInput;
            yield return ReachingTypeOutput;
            yield return ReachingWidth;
        }
    }
}
