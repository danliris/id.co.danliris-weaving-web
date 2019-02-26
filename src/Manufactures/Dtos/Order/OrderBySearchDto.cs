using Manufactures.Domain.Construction;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Order
{
    public class OrderBySearchDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "OrderStatus")]
        public string OrderStatus { get; private set; }

        [JsonProperty(PropertyName = "FabricConstructionDocument")]
        public ConstructionDocumentValueObject FabricConstructionDocument { get; private set; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; private set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; private set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; private set; }

        [JsonProperty(PropertyName = "WholeGrade")]
        public int WholeGrade { get; private set; }

        [JsonProperty(PropertyName = "YarnType")]
        public string YarnType { get; private set; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; private set; }

        [JsonProperty(PropertyName = "WarpComposition")]
        public Composition WarpComposition { get; private set; }

        [JsonProperty(PropertyName = "WeftComposition")]
        public Composition WeftComposition { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public WeavingUnit WeavingUnit { get; private set; }

        public OrderBySearchDto(WeavingOrderDocument weavingOrderDocument, ConstructionDocument constructionDocument)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            OrderStatus = weavingOrderDocument.OrderStatus;
            var construction = new ConstructionDocumentValueObject(constructionDocument.Identity, 
                                                                   constructionDocument.ConstructionNumber, 
                                                                   constructionDocument.TotalYarn);
            FabricConstructionDocument = construction;
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
