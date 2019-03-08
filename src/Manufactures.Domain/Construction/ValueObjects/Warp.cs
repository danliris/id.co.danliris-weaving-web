using Manufactures.Domain.Yarns;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class Warp : ValueObject
    {
        [JsonProperty(PropertyName = "Yarn")]
        public YarnValueObject Yarn { get; private set; }

        [JsonProperty(PropertyName = "Quantity")]
        public double Quantity { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }
        
        public Warp(double quantity,
                    string information,
                    YarnValueObject yarn)
        {
            this.Quantity = quantity;
            this.Information = information;
            this.Yarn = yarn;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Quantity;
            yield return Information;
            yield return Yarn;
        }
    }
}
