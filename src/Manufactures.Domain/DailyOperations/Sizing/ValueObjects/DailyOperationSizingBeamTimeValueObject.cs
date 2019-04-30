using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingBeamTimeValueObject : ValueObject
    {
        public DateTimeOffset? UpTime { get; set; }
        public DateTimeOffset? DownTime { get; set; }

        public DailyOperationSizingBeamTimeValueObject(DateTimeOffset? upTime, DateTimeOffset? downTime)
        {
            UpTime = upTime;
            DownTime = downTime;
        }

        public DailyOperationSizingBeamTimeValueObject(DailyOperationSizingBeamTimeCommand dailyOperationSizingBeam)
        {
            UpTime = dailyOperationSizingBeam.UpTime;
            DownTime = dailyOperationSizingBeam.DownTime;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UpTime;
            yield return DownTime;
        }
    }
}
