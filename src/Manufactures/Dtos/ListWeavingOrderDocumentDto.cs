using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class ListWeavingOrderDocumentDto
    {
        public ListWeavingOrderDocumentDto(WeavingOrderDocument weavingOrderDocument)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            ConstructionNumber = weavingOrderDocument.FabricConstructionDocument.ConstructionNumber;
            Composition = weavingOrderDocument.Composition;
            WeavingUnit = weavingOrderDocument.WeavingUnit;
        }

        public Guid Id { get; }
        public string OrderNumber { get; }
        public DateTimeOffset DateOrdered { get; }
        public string ConstructionNumber { get; }
        public Composition Composition { get; }
        public WeavingUnit WeavingUnit { get; }
    }
}
