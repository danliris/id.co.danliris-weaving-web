using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.ValueObjects
{
    public class EstimationProductValueObject : ValueObject
    {
        // From Construction Document
        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; private set; }

        // From SOP
        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "WholeGrade")]
        public int WholeGrade { get; private set; }

        // Calculation from totalyarn * TotalOrder From SOP
        [JsonProperty(PropertyName = "TotalGramEstimation")]
        public double TotalGramEstimation { get; private set; }

        // Product Grade
        [JsonProperty(PropertyName = "GradeA")]
        public int GradeA { get; private set; }

        [JsonProperty(PropertyName = "GradeB")]
        public int GradeB { get; private set; }

        [JsonProperty(PropertyName = "GradeC")]
        public int GradeC { get; private set; }

        [JsonProperty(PropertyName = "GradeD")]
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
