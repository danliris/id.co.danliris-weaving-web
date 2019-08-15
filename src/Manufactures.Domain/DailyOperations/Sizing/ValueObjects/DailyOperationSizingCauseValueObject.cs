using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingCauseValueObject : ValueObject
    {
        public string BrokenBeam { get; set; }
        
        public string MachineTroubled { get; set; }

        public DailyOperationSizingCauseValueObject()
        {
        }

        public DailyOperationSizingCauseValueObject(string brokenBeam, string machineTroubled)
        {
            BrokenBeam = brokenBeam;
            MachineTroubled = machineTroubled;
        }

        public DailyOperationSizingCauseValueObject(DailyOperationSizingCauseCommand dailyOperationSizingCausesProduction)
        {
            BrokenBeam = dailyOperationSizingCausesProduction.BrokenBeam;
            MachineTroubled = dailyOperationSizingCausesProduction.MachineTroubled;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BrokenBeam;
            yield return MachineTroubled;
        }
    }
}
