using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class Weft : ValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Quantity")]
        public double Quantity { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        [JsonProperty(PropertyName = "Yarn")]
        public Yarn Yarn { get; private set; }

        [JsonProperty(PropertyName = "Detail")]
        public string Detail { get; private set; }

        public Weft(Guid id,
                    double quantity,
                    string information,
                    Yarn yarn,
                    string detail)
        {
            this.Id = id;
            this.Quantity = quantity;
            this.Information = information;
            this.Yarn = yarn;
            this.Detail = detail;
        }
        
        public void SetDetail(string detail)
        {
            this.Detail = detail;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Quantity;
            yield return Information;
            yield return Yarn;
            yield return Detail;
        }
    }
}
