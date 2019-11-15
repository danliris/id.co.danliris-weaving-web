using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationReachingHistoryDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        public DailyOperationReachingHistoryDto(Guid id,
                                                string operatorName,
                                                int yarnStrandsProcessed,
                                                DateTimeOffset dateTimeMachine,
                                                string shiftName,
                                                string machineStatus)
        {
            Id = id;
            OperatorName = operatorName;
            YarnStrandsProcessed = yarnStrandsProcessed;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            MachineStatus = machineStatus;
        }
    }
}
