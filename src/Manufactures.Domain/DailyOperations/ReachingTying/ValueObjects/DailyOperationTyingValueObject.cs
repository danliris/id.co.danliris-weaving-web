using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.ValueObjects
{
    public class DailyOperationTyingValueObject : ValueObject
    {
        public int TyingWovenStrands { get; set; }
        public int TyingNumber { get; set; }
        public double TyingWidth { get; set; }

        public DailyOperationTyingValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TyingWovenStrands;
            yield return TyingNumber;
            yield return TyingWidth;
        }
    }
}
