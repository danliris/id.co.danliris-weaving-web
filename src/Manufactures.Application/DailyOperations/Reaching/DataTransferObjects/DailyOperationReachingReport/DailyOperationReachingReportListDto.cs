using Manufactures.Domain.DailyOperations.Reaching;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects.DailyOperationReachingReport
{
    public class DailyOperationReachingReportListDto
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

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; set; }

        [JsonProperty(PropertyName = "ReachingOperatorGroup")]
        public string ReachingOperatorGroup { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "LastModifiedTime")]
        public TimeSpan LastModifiedTime { get; set; }

        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get; set; }

        public DailyOperationReachingReportListDto(DailyOperationReachingDocument document, 
                                                   string machineNumber, 
                                                   string orderProductionNumber, 
                                                   string constructionNumber, 
                                                   string weavingUnit, 
                                                   string sizingBeamNumber, 
                                                   string operatorName, 
                                                   string reachingOperatorGroup, 
                                                   DateTimeOffset preparationDate, 
                                                   TimeSpan lastModifiedTime, 
                                                   string shift)
        {
            Id = document.Identity;
            MachineNumber = machineNumber;
            OrderProductionNumber = orderProductionNumber;
            ConstructionNumber = constructionNumber;
            WeavingUnit = weavingUnit;
            SizingBeamNumber = sizingBeamNumber;
            OperatorName = operatorName;
            ReachingOperatorGroup = reachingOperatorGroup;
            PreparationDate = preparationDate;
            LastModifiedTime = lastModifiedTime;
            Shift = shift;
        }
    }
}
