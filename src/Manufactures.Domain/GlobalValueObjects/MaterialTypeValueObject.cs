using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class MaterialTypeValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        public MaterialTypeValueObject(Guid id, 
                                       string code,
                                       string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Name;
        }
    }
}
