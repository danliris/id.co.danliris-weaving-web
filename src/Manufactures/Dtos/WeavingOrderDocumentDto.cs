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
            FabricConstruction = weavingOrderDocument.FabricConstruction;
            DateOrdered = weavingOrderDocument.DateOrdered;
            WarpOrigin = weavingOrderDocument.WarpOrigin;
            WeftOrigin = weavingOrderDocument.WeftOrigin;
            WholeGrade = weavingOrderDocument.WholeGrade;
            YarnType = weavingOrderDocument.YarnType;
            Period = weavingOrderDocument.Period;
            Composition = weavingOrderDocument.Composition;
            WeavingUnit = weavingOrderDocument.WeavingUnit;
            UserId = weavingOrderDocument.UserId;
        }

        public Guid Id { get; }
        public string OrderNumber { get; }
        public FabricConstruction FabricConstruction { get; }
        public DateTimeOffset DateOrdered { get; }
        public string WarpOrigin { get; }
        public string WeftOrigin { get; }
        public int WholeGrade { get; }
        public string YarnType { get; }
        public Period Period { get; }
        public Composition Composition { get; }
        public WeavingUnit WeavingUnit { get; }
        public string UserId { get; }
    }
}
