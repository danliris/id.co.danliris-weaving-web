using Moonlay.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.GlobalValueObjects 
{
    public class RingDocumentValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Number")]
        public int Number { get; private set; }

        public RingDocumentValueObject(string code, int number)
        {
            Code = code;
            Number = number;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Number;
        }
    }
}
