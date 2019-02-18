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
        public double TotalYarn { get; private set; }

        // From SOP
        public DateTimeOffset DateOrdered { get; private set; }
        public string OrderNumber { get; private set; }
        public int WholeGrade { get; private set; }

        // Calculation from totalyarn * TotalOrder From SOP
        public double TotalGramEstimation { get; private set; }

        // Product Grade
        public int GradeA { get; private set; }
        public int GradeB { get; private set; }
        public int GradeC { get; private set; }
        public int GradeD { get; private set; }
        
        public EstimationProductValueObject(string constructionNumber, 
                                            double totalYarn,
                                            DateTimeOffset dateOrdered,
                                            string orderNumber,
                                            int wholeGrade,
                                            double totalGramEstimation,
                                            int gradeA,
                                            int gradeB,
                                            int gradeC,
                                            int gradeD)
        {
            ConstructionNumber = constructionNumber;
            TotalYarn = totalYarn;
            DateOrdered = dateOrdered;
            OrderNumber = orderNumber;
            WholeGrade = wholeGrade;
            TotalGramEstimation = totalGramEstimation;
            GradeA = gradeA;
            GradeB = gradeB;
            GradeC = gradeC;
            GradeD = gradeD;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOrdered;
            yield return OrderNumber;
            yield return ConstructionNumber;
            yield return TotalGramEstimation;
            yield return TotalYarn;
            yield return WholeGrade;
            yield return GradeA;
            yield return GradeB;
            yield return GradeC;
            yield return GradeD;
        }
    }
}
