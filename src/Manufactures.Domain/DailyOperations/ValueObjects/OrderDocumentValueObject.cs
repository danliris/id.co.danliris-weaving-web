using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class OrderDocumentValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Identity")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; set; }

        public OrderDocumentValueObject(string orderNumber, string constructionNumber, string warpOrigin, string weftOrigin)
        {
            OrderNumber = orderNumber;
            ConstructionNumber = constructionNumber;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return OrderNumber;
            yield return ConstructionNumber;
            yield return WarpOrigin;
            yield return WeftOrigin;
        }
    }
}
