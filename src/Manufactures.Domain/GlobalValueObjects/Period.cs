using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class Period : ValueObject
    {
        public Period(string month, string year)
        {
            Month = month;
            Year = year;
        }

        public string Month { get; private set; }
        public string Year { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Month;
            yield return Year;
        }
    }
}
