using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Loom
{
    public class DailyOperationLoomHistoryDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "BeamOperatorName")]
        public string BeamOperatorName { get; }

        [JsonProperty(PropertyName = "BeamOperatorGroup")]
        public string BeamOperatorGroup { get; }

        [JsonProperty(PropertyName = "DateTimeOperation")]
        public DateTimeOffset DateTimeOperation { get; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; }

        public DailyOperationLoomHistoryDto(Guid id,
                                            string beamOperatorName,
                                            string beamOperatorGroup,
                                            DateTimeOffset dateTimeOperation,
                                            string operationStatus)
        {
            Id = id;
            BeamOperatorName = beamOperatorName;
            BeamOperatorGroup = beamOperatorGroup;
            DateTimeOperation = dateTimeOperation;
            OperationStatus = operationStatus;
        }
    }
}
