using System;
using Manufactures.Domain.DailyOperations.Warping;
using Newtonsoft.Json;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationWarpingListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateTimeOperation")]
        public DateTimeOffset DateTimeOperation { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "LatestBeamNumber")]
        public string LatestBeamNumber { get; private set; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; private set; }

        public DailyOperationWarpingListDto(DailyOperationWarpingDocument document)
        {
            Id = document.Identity;
            DateTimeOperation = document.DateTimeOperation;
            OperationStatus = document.DailyOperationStatus;
        }

        public void SetOrderNumber(string value)
        {
            OrderNumber = value;
        }

        public void SetConstructionNumber(string value)
        {
            ConstructionNumber = value;
        }

        public void SetLatestBeamNumber(string value)
        {
            LatestBeamNumber = value;
        }
    }
}
