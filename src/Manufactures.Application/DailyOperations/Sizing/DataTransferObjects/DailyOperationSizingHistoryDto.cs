using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects
{
    public class DailyOperationSizingHistoryDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        [JsonProperty(PropertyName = "CausesBrokenBeam")]
        public int CausesBrokenBeam { get; }

        [JsonProperty(PropertyName = "CausesMachineTroubled")]
        public int CausesMachineTroubled { get; }

        public DailyOperationSizingHistoryDto(Guid id,
                                              string sizingBeamNumber,
                                              DateTimeOffset dateTimeMachine,
                                              string shiftName,
                                              string operatorName,
                                              string operatorGroup,
                                              string machineStatus,
                                              string information,
                                              int causesBrokenBeam,
                                              int causesMachineTroubled)
        {
            Id = id;
            SizingBeamNumber = sizingBeamNumber;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            MachineStatus = machineStatus;
            Information = information;
            CausesBrokenBeam = causesBrokenBeam;
            CausesMachineTroubled = causesMachineTroubled;
        }
    }
}
