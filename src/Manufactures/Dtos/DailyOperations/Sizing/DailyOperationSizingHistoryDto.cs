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

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        public DailyOperationSizingHistoryDto()
        {
        }

        public DailyOperationSizingHistoryDto(DateTimeOffset dateTimeOperation, string machineStatus, string information)
        {
            DateTimeOperation = dateTimeOperation;
            MachineStatus = machineStatus;
            Information = information;
        }
    }
}
