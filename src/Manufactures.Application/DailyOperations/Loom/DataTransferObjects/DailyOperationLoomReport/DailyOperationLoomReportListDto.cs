using Manufactures.Domain.DailyOperations.Loom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects.DailyOperationLoomReport
{
    public class DailyOperationLoomReportListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "OrderProductionNumber")]
        public string OrderProductionNumber { get; set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public string WeavingUnit { get; set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "LastModifiedTime")]
        public TimeSpan LastModifiedTime { get; set; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; set; }

        public DailyOperationLoomReportListDto(DailyOperationLoomDocument document, 
                                               string orderProductionNumber,
                                               string constructionNumber, 
                                               string weavingUnit,
                                               string warpOrigin,
                                               string weftOrigin,
                                               DateTimeOffset preparationDate, 
                                               TimeSpan lastModifiedTime)
        {
            Id = document.Identity;
            OrderProductionNumber = orderProductionNumber;
            ConstructionNumber = constructionNumber;
            WeavingUnit = weavingUnit;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
            PreparationDate = preparationDate;
            LastModifiedTime = lastModifiedTime;
            OperationStatus = document.OperationStatus;
        }
    }
}
