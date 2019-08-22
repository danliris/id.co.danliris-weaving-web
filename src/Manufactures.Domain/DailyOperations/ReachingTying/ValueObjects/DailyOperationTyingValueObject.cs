using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.ValueObjects
{
    public class DailyOperationTyingValueObject : ValueObject
    {
        public int TyingEdgeStitching { get; set; }
        public int TyingNumber { get; set; }
        public double TyingWidth { get; set; }

        public DailyOperationTyingValueObject()
        {
        }

        public DailyOperationTyingValueObject(int tyingEdgeStitching, int tyingNumber, double tyingWidth)
        {
            TyingEdgeStitching = tyingEdgeStitching;
            TyingNumber = tyingNumber;
            TyingWidth = tyingWidth;
        }

        public DailyOperationTyingValueObject(int tyingEdgeStitching, int tyingNumber)
        {
            TyingEdgeStitching = tyingEdgeStitching;
            TyingNumber = tyingNumber;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TyingEdgeStitching;
            yield return TyingNumber;
            yield return TyingWidth;
        }
    }
}
