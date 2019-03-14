using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GlobalValueObjects 
{
    public class YarnNumberValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }
        
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Number")]
        public int Number { get; private set; }

        public YarnNumberValueObject(Guid id, string code, int number)
        {
            Id = id;
            Code = code;
            Number = number;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Number;
        }
    }
}
