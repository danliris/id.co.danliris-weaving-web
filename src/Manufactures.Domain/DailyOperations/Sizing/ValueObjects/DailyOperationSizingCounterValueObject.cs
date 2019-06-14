using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingCounterValueObject : ValueObject
    {
        public string Start { get; set; }
        public string Finish { get; set; }

        public DailyOperationSizingCounterValueObject(string start, string finish)
        {
            Start = start;
            Finish = finish;
        }

        public DailyOperationSizingCounterValueObject()
        {
        }

        public DailyOperationSizingCounterValueObject(DailyOperationSizingCounterCommand dailyOperationSizingCounterProduction)
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
