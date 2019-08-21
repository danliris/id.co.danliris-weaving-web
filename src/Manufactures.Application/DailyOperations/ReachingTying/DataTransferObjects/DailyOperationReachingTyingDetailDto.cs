using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationReachingTyingDetailDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        public DailyOperationReachingTyingDetailDto(Guid id, 
                                               string operatorName, 
                                               DateTimeOffset dateTimeMachine, 
                                               string shiftName, 
                                               string machineStatus)
        {
            Id = id;
            OperatorName = operatorName;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            MachineStatus = machineStatus;
        }
    }
}
