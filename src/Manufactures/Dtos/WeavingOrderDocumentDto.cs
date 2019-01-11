using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.Dtos
{
    public class WeavingOrderDocumentDto
    {
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
            Composition = weavingOrderDocument.Composition;
            WeavingUnit = weavingOrderDocument.WeavingUnit;
        }

        public Guid Id { get; }
        public string OrderNumber { get; }
        public FabricConstructionDocument FabricConstructionDocument { get; }
        public DateTimeOffset DateOrdered { get; }
        public string WarpOrigin { get; }
        public string WeftOrigin { get; }
        public int WholeGrade { get; }
        public string YarnType { get; }
        public Period Period { get; }
        public Composition Composition { get; }
        public WeavingUnit WeavingUnit { get; }
    }
}
