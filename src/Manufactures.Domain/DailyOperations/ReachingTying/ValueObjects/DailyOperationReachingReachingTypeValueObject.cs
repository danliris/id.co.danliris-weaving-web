using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.ValueObjects
{
    public class DailyOperationReachingReachingTypeValueObject : ValueObject
    {
        public string Input { get; set; }

        public string Output { get; set; }

        public DailyOperationReachingReachingTypeValueObject()
        {
        }

        public DailyOperationReachingReachingTypeValueObject(string input, string output)
        {
            Input = input;
            Output = output;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Input;
            yield return Output;
        }
    }
}
