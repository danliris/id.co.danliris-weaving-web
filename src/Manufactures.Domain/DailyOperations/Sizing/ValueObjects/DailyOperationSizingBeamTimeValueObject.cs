using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingBeamTimeValueObject : ValueObject
    {
        public DateTimeOffset Install { get; set; }
        public DateTimeOffset Uninstall { get; set; }

        public DailyOperationSizingBeamTimeValueObject(DateTimeOffset install, DateTimeOffset uninstall)
        {
            Install = install;
            Uninstall = uninstall;
        }

        public DailyOperationSizingBeamTimeValueObject(DailyOperationSizingBeamTimeCommand dailyOperationSizingBeam)
        {
            Install = dailyOperationSizingBeam.Install;
            Uninstall = dailyOperationSizingBeam.Uninstall;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Install;
            yield return Uninstall;
        }
    }
}
