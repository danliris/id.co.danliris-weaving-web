using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Order
{
    public class WeavingOrderById
    {
        public WeavingOrderById(WeavingOrderDocument weavingOrderDocument)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            FabricConstructionDocument = new ConstructionById(weavingOrderDocument.FabricConstructionDocument.Id, 
                                                              weavingOrderDocument.FabricConstructionDocument.ConstructionNumber, 
                                                              weavingOrderDocument.YarnType);
            DateOrdered = weavingOrderDocument.DateOrdered;
            WarpOrigin = weavingOrderDocument.WarpOrigin;
            WeftOrigin = weavingOrderDocument.WeftOrigin;
            WholeGrade = weavingOrderDocument.WholeGrade;
            Period = weavingOrderDocument.Period;
            Composition = weavingOrderDocument.Composition;
            WeavingUnit = weavingOrderDocument.WeavingUnit;
        }

        public Guid Id { get; }
        public string OrderNumber { get; }
        public ConstructionById FabricConstructionDocument { get; }
        public DateTimeOffset DateOrdered { get; }
        public string WarpOrigin { get; }
        public string WeftOrigin { get; }
        public int WholeGrade { get; }
        public Period Period { get; }
        public Composition Composition { get; }
        public WeavingUnit WeavingUnit { get; }
    }
}
