using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingHistoryDto
    {

        [JsonProperty(PropertyName = "DateTimeOperation")]
        public DateTimeOffset DateTimeOperation { get; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        public DailyOperationSizingHistoryDto()
        {
        }

        public DailyOperationSizingHistoryDto(DateTimeOffset dateTimeOperation, string operationStatus, string information)
        {
            DateTimeOperation = dateTimeOperation;
            OperationStatus = operationStatus;
            Information = information;
        }
    }
}
