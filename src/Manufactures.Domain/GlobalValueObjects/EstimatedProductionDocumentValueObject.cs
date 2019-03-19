using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class EstimatedProductionDocumentValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; }

        [JsonProperty(PropertyName = "GradeA")]
        public double GradeA { get; }

        [JsonProperty(PropertyName = "GradeB")]
        public double GradeB { get; }

        [JsonProperty(PropertyName = "GradeC")]
        public double GradeC { get; }

        [JsonProperty(PropertyName = "GradeD")]
        public double GradeD { get; }

        [JsonProperty(PropertyName = "WholeGrade")]
        public double WholeGrade { get; }

        public EstimatedProductionDocumentValueObject(Guid identity, double gradeA, double gradeB, double gradeC, double gradeD, double wholeGrade)
        {
            Identity = identity;
            GradeA = gradeA;
            GradeB = gradeB;
            GradeC = gradeC;
            GradeD = gradeD;
            WholeGrade = wholeGrade;
        }
    }
}
