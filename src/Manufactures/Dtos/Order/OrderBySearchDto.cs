using Manufactures.Domain.Construction;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Order
{
    public class OrderBySearchDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public ConstructionDocumentValueObject FabricConstructionDocument { get; set; }
        public DateTimeOffset DateOrdered { get; set; }
        public string WarpOrigin { get; set; }
        public string WeftOrigin { get; set; }
        public int WholeGrade { get; set; }
        public string YarnType { get; set; }
        public Period Period { get; set; }
        public Composition Composition { get; set; }
        public WeavingUnit WeavingUnit { get; set; }

        public OrderBySearchDto(WeavingOrderDocument weavingOrderDocument, ConstructionDocument constructionDocument)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
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
