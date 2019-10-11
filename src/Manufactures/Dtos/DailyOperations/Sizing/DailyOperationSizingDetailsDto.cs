using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingDetailsDto
    {
        [JsonProperty(PropertyName = "DateTimeMachineHistory")]
        public DateTimeOffset DateTimeMachineHistory { get; }

        [JsonProperty(PropertyName = "MachineStatusHistory")]
        public string MachineStatusHistory { get; }

        [JsonProperty(PropertyName = "InformationHistory")]
        public string InformationHistory { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "BrokenBeamCauses")]
        public int BrokenBeamCauses { get; }

        [JsonProperty(PropertyName = "MachineTroubledCauses")]
        public int MachineTroubledCauses { get; }

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; }

        public DailyOperationSizingDetailsDto(string operatorName, string operatorGroup, string shiftName, DailyOperationSizingDetailsHistoryDto history, DailyOperationSizingDetailsCausesDto causes, string sizingBeamNumber)
        {
            DateTimeMachineHistory = history.DateTimeOperation;
            MachineStatusHistory = history.MachineStatus;
            InformationHistory = history.Information;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            ShiftName = shiftName;
            BrokenBeamCauses = causes.BrokenBeam;
            MachineTroubledCauses = causes.MachineTroubled;
            SizingBeamNumber = sizingBeamNumber;
        }
    }
}
