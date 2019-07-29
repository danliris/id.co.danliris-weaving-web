﻿using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingDetailsDto
    {
        [JsonProperty(PropertyName = "DateTimeDoff")]
        public DateTimeOffset DateTimeMachineHistory { get; }

        [JsonProperty(PropertyName = "MachineStatusHistory")]
        public string MachineStatusHistory { get; }

        [JsonProperty(PropertyName = "InformationHistory")]
        public string InformationHistory { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "BrokenBeamCauses")]
        public string BrokenBeamCauses { get; }

        [JsonProperty(PropertyName = "MachineTroubledCauses")]
        public string MachineTroubledCauses { get; }

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; }

        public DailyOperationSizingDetailsDto(string shiftName, DailyOperationSizingDetailsHistoryDto history, DailyOperationSizingDetailsCausesDto causes, string sizingBeamNumber)
        {
            DateTimeMachineHistory = history.DateTimeOperation;
            MachineStatusHistory = history.MachineStatus;
            InformationHistory = history.Information;
            ShiftName = shiftName;
            BrokenBeamCauses = causes.BrokenBeam;
            MachineTroubledCauses = causes.MachineTroubled;
            SizingBeamNumber = sizingBeamNumber;
        }
    }
}
