using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class YarnValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "MaterialCode")]
        public string MaterialCode { get; private set; }

        [JsonProperty(PropertyName = "RingCode")]
        public string RingCode { get; private set; }

        public YarnValueObject(Guid id,
                               string code,
                               string name,
                               string materialCode,
                               string ringCode)
        {
            Id = id;
            Code  = code;
            Name = name;
            MaterialCode = materialCode;
            RingCode = ringCode;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Name;
            yield return MaterialCode;
            yield return RingCode;
        }
    }
}
