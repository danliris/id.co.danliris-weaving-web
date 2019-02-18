using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.ValueObjects
{
    public class EstimationProductValueObject : ValueObject
    {
        // From Construction Document
        public string ConstructionNumber { get; set; }
        public double AmountTotal { get; set; }

        // From SOP
        public DateTimeOffset DateOrdered { get; set; }
        public string OrderNumber { get; set; }
        public int OrderTotal { get; set; }

        // Calculation from totalyarn * TotalOrder From SOP
        public double TotalGramEstimation { get; set; }

        // Product Grade
        public int GradeA { get; set; }
        public int GradeB { get; set; }
        public int GradeC { get; set; }
        public int GradeD { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOrdered;
            yield return OrderNumber;
            yield return ConstructionNumber;
            yield return TotalGramEstimation;
            yield return AmountTotal;
            yield return OrderTotal;
            yield return GradeA;
            yield return GradeB;
            yield return GradeC;
            yield return GradeD;
        }
    }
}
