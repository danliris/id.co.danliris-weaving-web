using Manufactures.Domain.DailyOperations.Warping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.DailyOperationWarpingReport
{
    public class DailyOperationWarpingReportListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "OrderProductionNumber")]
        public string OrderProductionNumber { get; set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public string WeavingUnit { get; set; }

        [JsonProperty(PropertyName = "MaterialType")]
        public string MaterialType { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public double AmountOfCones { get; set; }

        [JsonProperty(PropertyName = "ColourOfCones")]
        public string ColourOfCones { get; set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; set; }

        [JsonProperty(PropertyName = "WarpingOperatorGroup")]
        public string WarpingOperatorGroup { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "LastModifiedTime")]
        public TimeSpan LastModifiedTime { get; set; }

        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get; set; }

        public DailyOperationWarpingReportListDto(DailyOperationWarpingDocument document, 
                                                  string orderProductionNumber, 
                                                  string constructionNumber, 
                                                  string weavingUnit, 
                                                  string materialType, 
                                                  string operatorName, 
                                                  string warpingOperatorGroup, 
                                                  DateTimeOffset preparationDate,
                                                  TimeSpan lastModifiedTime,
                                                  string shift)
        {
            Id = document.Identity;
            OrderProductionNumber = orderProductionNumber;
            ConstructionNumber = constructionNumber;
            WeavingUnit = weavingUnit;
            MaterialType = materialType;
            AmountOfCones = document.AmountOfCones;
            ColourOfCones = document.ColourOfCone;
            OperatorName = operatorName;
            WarpingOperatorGroup = warpingOperatorGroup;
            PreparationDate = preparationDate;
            LastModifiedTime = lastModifiedTime;
            Shift = shift;
        }
    }
}
