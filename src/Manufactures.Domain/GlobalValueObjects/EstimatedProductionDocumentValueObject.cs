using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class EstimatedProductionDocumentValueObject
    {
        [JsonProperty(PropertyName = "GradeA")]
        public int GradeA { get; }

        [JsonProperty(PropertyName = "GradeB")]
        public int GradeB { get; }

        [JsonProperty(PropertyName = "GradeC")]
        public int GradeC { get; }

        [JsonProperty(PropertyName = "GradeD")]
        public int GradeD { get; }

        [JsonProperty(PropertyName = "WholeGrade")]
        public int WholeGrade { get; }

        public EstimatedProductionDocumentValueObject(int gradeA, int gradeB, int gradeC, int gradeD, int wholeGrade)
        {
            GradeA = gradeA;
            GradeB = gradeB;
            GradeC = gradeC;
            GradeD = gradeD;
            WholeGrade = wholeGrade;
        }
    }
}
