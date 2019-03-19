using Manufactures.Domain.Construction;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Order
{
    public class OrderReportBySearch
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "OrderStatus")]
        public string OrderStatus { get; private set; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public UnitId UnitId { get; private set; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; private set; }

        //[JsonProperty(PropertyName = "YarnType")]
        //public string YarnType { get; private set; }

        [JsonProperty(PropertyName = "WarpComposition")]
        public Composition WarpComposition { get; private set; }

        [JsonProperty(PropertyName = "WeftComposition")]
        public Composition WeftComposition { get; private set; }

        [JsonProperty(PropertyName = "FabricConstructionDocument")]
        public ConstructionDocumentValueObject FabricConstructionDocument { get; private set; }

        [JsonProperty(PropertyName = "YarnNumber")]
        public string YarnNumber { get; private set; }

        //[JsonProperty(PropertyName = "WarpOrigin")]
        //public string WarpOrigin { get; private set; }

        //[JsonProperty(PropertyName = "WeftOrigin")]
        //public string WeftOrigin { get; private set; }

        [JsonProperty(PropertyName = "EstimatedProductionDocument")]
        public EstimatedProductionDocumentValueObject EstimatedProductionDocument { get; private set; }

        public OrderReportBySearch(WeavingOrderDocument weavingOrderDocument, ConstructionDocument constructionDocument, EstimatedProductionDocument estimatedProduction)
        {
            Id = weavingOrderDocument.Identity;
            OrderNumber = weavingOrderDocument.OrderNumber;
            OrderStatus = weavingOrderDocument.OrderStatus;
            Period = weavingOrderDocument.Period;
            UnitId = weavingOrderDocument.UnitId;
            DateOrdered = weavingOrderDocument.DateOrdered;
            //YarnType = weavingOrderDocument.YarnType;
            WarpComposition = weavingOrderDocument.WarpComposition;
            WeftComposition = weavingOrderDocument.WeftComposition;
            //WarpOrigin = weavingOrderDocument.WarpOrigin;
            //WeftOrigin = weavingOrderDocument.WeftOrigin
            var construction = new ConstructionDocumentValueObject(constructionDocument.Identity,
                                                                    constructionDocument.ConstructionNumber,
                                                                    constructionDocument.AmountOfWarp,
                                                                    constructionDocument.AmountOfWeft,
                                                                    constructionDocument.TotalYarn);
            FabricConstructionDocument = construction;
            foreach (var item in estimatedProduction.EstimationProducts)
            {
                //var query = item
            }
            //var estimatedproduction = new EstimatedProductionDocumentValueObject(estimatedProduction.Identity, estimatedProduction.GradeA, estimatedProduction.GradeB, estimatedProduction.GradeC, estimatedProduction.GradeD, estimatedProduction.WholeGrade);
            //EstimatedProductionDocument = estimatedproduction;
        }
    }
}
