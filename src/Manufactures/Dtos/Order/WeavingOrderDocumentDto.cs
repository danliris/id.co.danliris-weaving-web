using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Order
{
    public class WeavingOrderDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "FabricConstructionDocument")]
        public FabricConstructionDocument FabricConstructionDocument { get; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; }

        [JsonProperty(PropertyName = "WholeGrade")]
        public int WholeGrade { get; }

        [JsonProperty(PropertyName = "YarnType")]
        public string YarnType { get; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; }

        [JsonProperty(PropertyName = "WarpComposition")]
        public Composition WarpComposition { get; }

        [JsonProperty(PropertyName = "WeftComposition")]
        public Composition WeftComposition { get; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public WeavingUnit WeavingUnit { get; }

        public WeavingOrderDocumentDto(WeavingOrderDocument weavingOrderDocument)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            FabricConstructionDocument = weavingOrderDocument.FabricConstructionDocument;
            DateOrdered = weavingOrderDocument.DateOrdered;
            WarpOrigin = weavingOrderDocument.WarpOrigin;
            WeftOrigin = weavingOrderDocument.WeftOrigin;
            WholeGrade = weavingOrderDocument.WholeGrade;
            YarnType = weavingOrderDocument.YarnType;
            Period = weavingOrderDocument.Period;
            WarpComposition = weavingOrderDocument.WarpComposition;
            WeftComposition = weavingOrderDocument.WeftComposition;
            WeavingUnit = weavingOrderDocument.WeavingUnit;
        }
    }
}
