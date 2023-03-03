using Manufactures.Domain.DailyOperations.Sizing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.DailyOperationSizingReport
{
    public class DailyOperationSizingReportListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "OrderProductionNumber")]
        public string OrderProductionNumber { get; set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public string WeavingUnit { get; set; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; set; }

        [JsonProperty(PropertyName = "NeReal")]
        public double NeReal { get; set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; set; }

        [JsonProperty(PropertyName = "SizingOperatorGroup")]
        public string SizingOperatorGroup { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "LastModifiedTime")]
        public TimeSpan LastModifiedTime { get; set; }

        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get; set; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; set; }

        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; set; }

        public DailyOperationSizingReportListDto(DailyOperationSizingDocument document,
                                                 string machineNumber, 
                                                 string orderProductionNumber, 
                                                 string constructionNumber, 
                                                 string weavingUnit, 
                                                 string operatorName, 
                                                 string sizingOperatorGroup, 
                                                 DateTimeOffset preparationDate, 
                                                 TimeSpan lastModifiedTime,
                                                 string shift, 
                                                 double yarnStrands, 
                                                 double emptyWeight)
        {
            Id = document.Identity;
            MachineNumber = machineNumber;
            OrderProductionNumber = orderProductionNumber;
            ConstructionNumber = constructionNumber;
            WeavingUnit = weavingUnit;
            RecipeCode = document.RecipeCode;
            NeReal = document.NeReal;
            OperatorName = operatorName;
            SizingOperatorGroup = sizingOperatorGroup;
            PreparationDate = preparationDate;
            LastModifiedTime = lastModifiedTime;
            Shift = shift;
            YarnStrands = yarnStrands;
            EmptyWeight = emptyWeight;
        }
    }
}
