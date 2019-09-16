using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationWarpingHistoryDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "WarpingBeamNumber")]
        public string WarpingBeamNumber { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeOperation { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; }

        public DailyOperationWarpingHistoryDto(Guid id,
                                               string warpingBeamNumber,
                                               DateTimeOffset dateTimeOperation,
                                               string shiftName,
                                               string operatorName,
                                               string operatorGroup,
                                               string operationStatus)
        {
            Id = id;
            WarpingBeamNumber = warpingBeamNumber;
            DateTimeOperation = dateTimeOperation;
            ShiftName = shiftName;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            OperationStatus = operationStatus;
        }
    }
}
