using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingDetailsDto
    {
        [JsonProperty(PropertyName = "History")]
        public DailyOperationSizingHistoryDto History { get; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; }

        [JsonProperty(PropertyName = "Causes")]
        public DailyOperationSizingCausesDto Causes { get; }

        public DailyOperationSizingDetailsDto(DailyOperationSizingDetail detail)
        {
            History = detail.History.Deserialize<DailyOperationSizingHistoryDto>();
            ShiftDocumentId = new ShiftId(detail.ShiftDocumentId);
            Causes = detail.Causes.Deserialize<DailyOperationSizingCausesDto>();
        }
    }
}
