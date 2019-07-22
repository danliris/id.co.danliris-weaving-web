using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingDetailsHistoryDto
    {

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeOperation { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        public DailyOperationSizingDetailsHistoryDto()
        {
        }

        public DailyOperationSizingDetailsHistoryDto(DateTimeOffset dateTimeOperation, string machineStatus, string information)
        {
            DateTimeOperation = dateTimeOperation;
            MachineStatus = machineStatus;
            Information = information;
        }
    }
}
