using Moonlay.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class Period : ValueObject
    {
        [JsonProperty(PropertyName = "Month")]
        public string Month { get; private set; }

        [JsonProperty(PropertyName = "Year")]
        public string Year { get; private set; }

        public Period(string month, string year)
        {
            Month = month;
            Year = year;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Month;
            yield return Year;
        }
    }
}
