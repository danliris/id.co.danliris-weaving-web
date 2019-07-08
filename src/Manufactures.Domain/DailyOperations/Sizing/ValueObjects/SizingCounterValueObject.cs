using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class SizingCounterValueObject : ValueObject
    {
        public double Start { get; set; }
        public double Finish { get; set; }

        public SizingCounterValueObject(double start, double finish)
        {
            Start = start;
            Finish = finish;
        }

        public SizingCounterValueObject()
        {
        }

        public SizingCounterValueObject(SizingCounterCommand dailyOperationSizingCounterProduction)
        {
            Start = dailyOperationSizingCounterProduction.Start;
            Finish = dailyOperationSizingCounterProduction.Finish;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Start;
            yield return Finish;
        }
    }
}
