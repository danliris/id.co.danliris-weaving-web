﻿using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.Order
{
    public class ListWeavingOrderDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; }

        [JsonProperty(PropertyName = "ConstructionId")]
        public ConstructionId ConstructionId { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "WarpComposition")]
        public Composition WarpComposition { get; }

        [JsonProperty(PropertyName = "WeftComposition")]
        public Composition WeftComposition { get; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public UnitId UnitId { get; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; }

        public ListWeavingOrderDocumentDto(OrderDocument weavingOrderDocument, 
                                           FabricConstructionDocument fabricConstruction)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            DateOrdered = weavingOrderDocument.DateOrdered;
            ConstructionId = new ConstructionId(fabricConstruction.Id);
            ConstructionNumber = fabricConstruction.ConstructionNumber;
            WarpComposition = weavingOrderDocument.WarpComposition;
            WeftComposition = weavingOrderDocument.WeftComposition;
            UnitId = weavingOrderDocument.UnitId;
            WarpOrigin = weavingOrderDocument.WarpOrigin;
            WeftOrigin = weavingOrderDocument.WeftOrigin;
        }
    }
}
