using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Order
{
    public class ListWeavingOrderDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "WarpComposition")]
        public Composition WarpComposition { get; }

        [JsonProperty(PropertyName = "WeftComposition")]
        public Composition WeftComposition { get; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public WeavingUnit WeavingUnit { get; }

        public ListWeavingOrderDocumentDto(WeavingOrderDocument weavingOrderDocument)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            DateOrdered = weavingOrderDocument.DateOrdered;
            ConstructionNumber = weavingOrderDocument.FabricConstructionDocument.ConstructionNumber;
            WarpComposition = weavingOrderDocument.WarpComposition;
            WeftComposition = weavingOrderDocument.WeftComposition;
            WeavingUnit = weavingOrderDocument.WeavingUnit;
        }
    }
}
