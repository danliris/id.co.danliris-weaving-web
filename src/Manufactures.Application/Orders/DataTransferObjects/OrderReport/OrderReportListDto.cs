using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Application.Orders.DataTransferObjects.OrderReport
{
    public class OrderReportListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "OrderStatus")]
        public string OrderStatus { get; private set; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnitName")]
        public string WeavingUnitName { get; private set; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; private set; }

        [JsonProperty(PropertyName = "WarpComposition")]
        public Composition WarpComposition { get; private set; }

        [JsonProperty(PropertyName = "WeftComposition")]
        public Composition WeftComposition { get; private set; }

        [JsonProperty(PropertyName = "FabricConstructionDocument")]
        public ConstructionDocumentValueObject FabricConstructionDocument { get; private set; }

        [JsonProperty(PropertyName = "YarnNumber")]
        public string YarnNumber { get; private set; }

        [JsonProperty(PropertyName = "WholeGrade")]
        public int WholeGrade { get; private set; }

        [JsonProperty(PropertyName = "EstimatedProductionDocument")]
        public EstimatedProductionDocumentValueObject EstimatedProductionDocument { get; private set; }

        public OrderReportListDto(OrderDocument weavingOrderDocument, 
                                  Domain.FabricConstructions.FabricConstructionDocument constructionDocument, 
                                  List<EstimatedProductionDocument> estimationDocument, 
                                  string yarnNumber, 
                                  string unit)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            OrderStatus = weavingOrderDocument.OrderStatus;
            Period = weavingOrderDocument.Period;
            WholeGrade = weavingOrderDocument.WholeGrade;
            WeavingUnitName = unit;
            DateOrdered = weavingOrderDocument.DateOrdered;
            WarpComposition = weavingOrderDocument.WarpComposition;
            WeftComposition = weavingOrderDocument.WeftComposition;

            var construction = new ConstructionDocumentValueObject(constructionDocument.Identity,
                                                                   constructionDocument.ConstructionNumber,
                                                                   constructionDocument.AmountOfWarp,
                                                                   constructionDocument.AmountOfWeft,
                                                                   constructionDocument.TotalYarn);
            FabricConstructionDocument = construction;
            YarnNumber = yarnNumber;
            foreach (var item in estimationDocument)
            {
                foreach (var datum in item.EstimationProducts)
                {
                    if (datum.OrderDocument.Deserialize<OrderDocumentValueObject>().OrderNumber.Equals(weavingOrderDocument.OrderNumber))
                    {
                        var productGrade = datum.ProductGrade.Deserialize<ProductGrade>();
                        EstimatedProductionDocument = new EstimatedProductionDocumentValueObject(productGrade.GradeA, productGrade.GradeB, productGrade.GradeC, productGrade.GradeD, weavingOrderDocument.WholeGrade);
                    }
                }
            }
        }
    }
}
