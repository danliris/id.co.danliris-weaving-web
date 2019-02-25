using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.Dtos.Order
{
    public class ListWeavingOrderDocumentDto
    {
        public Guid Id { get; }
        public string OrderNumber { get; }
        public DateTimeOffset DateOrdered { get; }
        public string ConstructionNumber { get; }
        public Composition WarpComposition { get; }
        public Composition WeftComposition { get; }
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
