using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.Order
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
        public UnitId UnitId { get; }

        public WeavingOrderDocumentDto(OrderDocument weavingOrderDocument,
                                       UnitId unitId,
                                       FabricConstructionDocument fabricConstruction)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            FabricConstructionDocument = fabricConstruction;
            DateOrdered = weavingOrderDocument.DateOrdered;
            WarpOrigin = weavingOrderDocument.WarpOrigin;
            WeftOrigin = weavingOrderDocument.WeftOrigin;
            WholeGrade = weavingOrderDocument.WholeGrade;
            YarnType = weavingOrderDocument.YarnType;
            Period = weavingOrderDocument.Period;
            WarpComposition = weavingOrderDocument.WarpComposition;
            WeftComposition = weavingOrderDocument.WeftComposition;
            UnitId = weavingOrderDocument.UnitId;
        }
    }
}
