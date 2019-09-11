using System;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationWarpingListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public int WeavingUnitDocumentId { get; private set; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; private set; }

        public DailyOperationWarpingListDto(DailyOperationWarpingDocument document)
        {
            Id = document.Identity;
            DateTimeMachine = document.DateTimeOperation;
            OperationStatus = document.OperationStatus;
        }

        public void SetOrderNumber(string orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public void SetConstructionNumber(string constructionNumber)
        {
            ConstructionNumber = constructionNumber;
        }

        public void SetWeavingUnitDocumentId(int weavingUnitDocumentId)
        {
            WeavingUnitDocumentId = weavingUnitDocumentId;
        }
    }
}
