using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.Orders;
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

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; private set; }

        [JsonProperty(PropertyName = "YarnNumber")]
        public string YarnNumber { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionPoly")]
        public double WarpCompositionPoly { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionCotton")]
        public double WarpCompositionCotton { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionOthers")]
        public double WarpCompositionOthers { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionPoly")]
        public double WeftCompositionPoly { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionCotton")]
        public double WeftCompositionCotton { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionOthers")]
        public double WeftCompositionOthers { get; private set; }

        [JsonProperty(PropertyName = "EstimatedProductionGradeA")]
        public double EstimatedProductionGradeA { get; private set; }

        [JsonProperty(PropertyName = "EstimatedProductionGradeB")]
        public double EstimatedProductionGradeB { get; private set; }

        [JsonProperty(PropertyName = "EstimatedProductionGradeC")]
        public double EstimatedProductionGradeC { get; private set; }

        [JsonProperty(PropertyName = "EstimatedProductionGradeD")]
        public double EstimatedProductionGradeD { get; private set; }

        [JsonProperty(PropertyName = "TotalEstimatedProduction")]
        public double TotalEstimatedProduction { get; private set; }

        [JsonProperty(PropertyName = "AmountOfWarp")]
        public int AmountOfWarp { get; private set; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public int AmountOfWeft { get; private set; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; private set; }

        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; private set; }

        public OrderReportListDto(OrderDocument orderDocument, 
                                  FabricConstructionDocument fabricConstructionDocument,
                                  EstimatedProductionDetail estimationDetail,
                                  string unitName)
        {
            Id = orderDocument.Identity;
            OrderNumber = orderDocument.OrderNumber;
            DateOrdered = orderDocument.AuditTrail.CreatedDate;
            YarnNumber = orderDocument.YarnType;
            WarpCompositionPoly = orderDocument.WarpCompositionPoly;
            WarpCompositionCotton = orderDocument.WarpCompositionCotton;
            WarpCompositionOthers = orderDocument.WarpCompositionOthers;
            WeftCompositionPoly = orderDocument.WeftCompositionPoly;
            WeftCompositionCotton = orderDocument.WeftCompositionCotton;
            WeftCompositionOthers = orderDocument.WeftCompositionOthers;
            ConstructionNumber = fabricConstructionDocument.ConstructionNumber;
            AmountOfWarp = fabricConstructionDocument.AmountOfWarp;
            AmountOfWeft = fabricConstructionDocument.AmountOfWeft;
            TotalYarn = fabricConstructionDocument.TotalYarn;
            EstimatedProductionGradeA = estimationDetail.GradeA;
            EstimatedProductionGradeB = estimationDetail.GradeB;
            EstimatedProductionGradeC = estimationDetail.GradeC;
            EstimatedProductionGradeD = estimationDetail.GradeD;
            TotalEstimatedProduction = orderDocument.AllGrade;
            UnitName = unitName;
        }
    }
}
