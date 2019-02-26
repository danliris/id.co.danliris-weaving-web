using Moonlay.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.GlobalValueObjects
{ 
    public class WeavingUnit : ValueObject
    {
        [JsonProperty(PropertyName = "_id")]
        public string _id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        public WeavingUnit(string _id, string code, string name)
        {
            this._id = _id;
            Code = code;
            Name = name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _id;
            yield return Code;
            yield return Name;
        }
    }
}
