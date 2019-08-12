using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationLoomHistoryDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }


        [JsonProperty(PropertyName = "BeamOperatorName")]
        public string BeamOperatorName { get; }

        [JsonProperty(PropertyName = "BeamOperatorGroup")]
        public string BeamOperatorGroup { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeOperation { get; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        public DailyOperationLoomHistoryDto(Guid id,
                                            string beamNumber,
                                            string beamOperatorName,
                                            string beamOperatorGroup,
                                            DateTimeOffset dateTimeOperation,
                                            string operationStatus,
                                            string shiftName)
        {
            Id = id;
            BeamNumber = beamNumber;
            BeamOperatorName = beamOperatorName;
            BeamOperatorGroup = beamOperatorGroup;
            DateTimeOperation = dateTimeOperation;
            OperationStatus = operationStatus;
            ShiftName = shiftName;
        }
    }
}
