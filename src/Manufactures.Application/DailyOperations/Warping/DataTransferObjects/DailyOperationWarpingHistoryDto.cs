using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class DailyOperationWarpingHistoryDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "WarpingBeamNumber")]
        public string WarpingBeamNumber { get; }

        [JsonProperty(PropertyName ="WarpingBeamId")]
        public string WarpingBeamId { get; set; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; }

        [JsonProperty(PropertyName = "WarpingBeamLengthPerOperator")]
        public string WarpingBeamLengthPerOperator { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        public DailyOperationWarpingHistoryDto(Guid id,
                                               string warpingBeamNumber,
                                               DateTimeOffset dateTimeMachine,
                                               string shiftName,
                                               string operatorName,
                                               string operatorGroup,
                                               string warpingBeamLengthPerOperator,
                                               string machineStatus)
        {
            Id = id;
            WarpingBeamNumber = warpingBeamNumber;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            WarpingBeamLengthPerOperator = warpingBeamLengthPerOperator;
            MachineStatus = machineStatus;
        }
        public DailyOperationWarpingHistoryDto(Guid id,
                                               string warpingBeamNumber,
                                               string warpingBeamId,
                                               DateTimeOffset dateTimeMachine,
                                               string shiftName,
                                               string operatorName,
                                               string operatorGroup,
                                               string warpingBeamLengthPerOperator,
                                               string machineStatus)
        {
            Id = id;
            WarpingBeamNumber = warpingBeamNumber;
            WarpingBeamId = warpingBeamId;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            WarpingBeamLengthPerOperator = warpingBeamLengthPerOperator;
            MachineStatus = machineStatus;
        }
    }
}
