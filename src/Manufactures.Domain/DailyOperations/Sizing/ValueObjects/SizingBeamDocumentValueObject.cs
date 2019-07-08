using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class SizingBeamDocumentValueObject : ValueObject
    {
        public BeamId SizingBeamId { get; set; }

        public SizingCounterValueObject Counter { get; set; }

        public SizingWeightValueObject Weight { get; private set; }

        public double SPU { get; private set; }

        public SizingBeamDocumentValueObject()
        {
        }

        public SizingBeamDocumentValueObject(BeamId sizingBeamId, SizingCounterValueObject counter, SizingWeightValueObject weight, double sPU)
        {
            SizingBeamId = sizingBeamId;
            Counter = counter;
            Weight = weight;
            SPU = sPU;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SizingBeamId;
            yield return Counter;
            yield return Weight;
            yield return SPU;
        }
    }
}
