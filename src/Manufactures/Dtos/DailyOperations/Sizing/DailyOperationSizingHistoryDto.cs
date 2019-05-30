using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingHistoryDto
    {
        [JsonProperty(PropertyName = "MachineDate")]
        public DateTimeOffset MachineDate { get; }

        [JsonProperty(PropertyName = "MachineTime")]
        public TimeSpan MachineTime { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        public DailyOperationSizingHistoryDto(DateTimeOffset machineDate, TimeSpan machineTime, string machineStatus, string information)
        {
            MachineDate = machineDate;
            MachineTime = machineTime;
            MachineStatus = machineStatus;
            Information = information;
        }
    }
}
