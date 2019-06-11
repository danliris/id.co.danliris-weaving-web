using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingDetailsDto
    {
        [JsonProperty(PropertyName = "DateTimeOperationHistory")]
        public DateTimeOffset DateTimeOperationHistory { get; }

        [JsonProperty(PropertyName = "OperationStatusHistory")]
        public string OperationStatusHistory { get; }

        [JsonProperty(PropertyName = "InformationHistory")]
        public string InformationHistory { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "BrokenBeamCauses")]
        public string BrokenBeamCauses { get; }

        [JsonProperty(PropertyName = "MachineTroubledCauses")]
        public string MachineTroubledCauses { get; }

        public DailyOperationSizingDetailsDto(string shiftName, DailyOperationSizingHistoryDto history, DailyOperationSizingCausesDto causes)
        {
            DateTimeOperationHistory = history.DateTimeOperation;
            OperationStatusHistory = history.OperationStatus;
            InformationHistory = history.Information;
            ShiftName = shiftName;
            BrokenBeamCauses = causes.BrokenBeam;
            MachineTroubledCauses = causes.MachineTroubled;
        }
    }
}
