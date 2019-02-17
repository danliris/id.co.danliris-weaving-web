using Manufactures.Domain.Construction;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.Dtos.Order
{
    public class OrderBySearchDto
    {
        public Guid Id { get; private set; }
        public string OrderNumber { get; private set; }
        public string OrderStatus { get; private set; }
        public ConstructionDocumentValueObject FabricConstructionDocument { get; private set; }
        public DateTimeOffset DateOrdered { get; private set; }
        public string WarpOrigin { get; private set; }
        public string WeftOrigin { get; private set; }
        public int WholeGrade { get; private set; }
        public string YarnType { get; private set; }
        public Period Period { get; private set; }
        public Composition Composition { get; private set; }
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
            Composition = weavingOrderDocument.Composition;
            WeavingUnit = weavingOrderDocument.WeavingUnit;
        }
    }
}
