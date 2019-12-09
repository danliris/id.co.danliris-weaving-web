using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.ValueObjects
{
    public class ProductGrade : ValueObject
    {
        public ProductGrade(int gradeA, int gradeB, int gradeC, int gradeD)
        {
            GradeA = gradeA;
            GradeB = gradeB;
            GradeC = gradeC;
            GradeD = gradeD;
        }

        public int GradeA { get; private set; }
        public int GradeB { get; private set; }
        public int GradeC { get; private set; }
        public int GradeD { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return GradeA;
            yield return GradeB;
            yield return GradeC;
            yield return GradeD;
        }
    }
}
