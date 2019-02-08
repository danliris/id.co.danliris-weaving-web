using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.ValueObjects
{
    public class EstimationProductValueObject : ValueObject
    {
        // From Construction Document
        public string ConstructionNumber { get; private set; }
        public double AmountTotal { get; private set; }

        // From SOP
        public DateTimeOffset DateOrdered { get; private set; }
        public string OrderNumber { get; private set; }
        public int OrderTotal { get; private set; }

        // Product Grade
        public int GradeA { get; private set; }
        public int GradeB { get; private set; }
        public int GradeC { get; private set; }
        public int GradeD { get; private set; }

        public void SetConstructionNumber(string value)
        {
            ConstructionNumber = value;
        }

        public void SetAmountTotal(double value)
        {
            AmountTotal = value;
        }

        public void SetDateOrdered(DateTimeOffset value)
        {
            DateOrdered = value;
        }

        public void SetOrderNumber(string value)
        {
            OrderNumber = value;
        }

        public void SetOrderTotal(int value)
        {
            OrderTotal = value;
        }

        public void SetGradeA(int value)
        {
            GradeA = value;
        }

        public void SetGradeB(int value)
        {
            GradeB = value;
        }

        public void SetGradeC(int value)
        {
            GradeC = value;
        }

        public void SetGradeD(int value)
        {
            GradeD = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOrdered;
            yield return OrderNumber;
            yield return ConstructionNumber;
            yield return AmountTotal;
            yield return OrderTotal;
            yield return GradeA;
            yield return GradeB;
            yield return GradeC;
            yield return GradeD;
        }
    }
}
