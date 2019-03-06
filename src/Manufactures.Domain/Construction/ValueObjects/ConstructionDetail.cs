using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class ConstructionDetail : ValueObject
    {
        [JsonProperty(PropertyName = "YarnId")]
        public YarnId YarnId { get; private set; }

        [JsonProperty(PropertyName = "Quantity")]
        public double Quantity { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        public ConstructionDetail(YarnId yarnId,
                                  double quantity,
                                  string information)
        {
            //Set Value
            Quantity = quantity;
            Information = information;
            YarnId = yarnId;
        }

        public void SetQuantity(double quantity)
        {
            if (quantity != Quantity)
            {
                Quantity = quantity;
            }
        }

        public void SetInformation(string information)
        {
            if (information != Information)
            {
                Information = information;
            }
        }

        public void SetYarn(YarnId value)
        {
            if (YarnId != value)
            {
                YarnId = value;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return YarnId;
            yield return Quantity;
            yield return Information;
        }
    }
}
