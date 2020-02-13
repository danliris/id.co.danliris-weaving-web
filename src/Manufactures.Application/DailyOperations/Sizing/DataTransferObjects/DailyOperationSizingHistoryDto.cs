using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Operators;
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

        [JsonProperty(PropertyName = "BrokenPerShift")]
        public int BrokenPerShift { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        public DailyOperationSizingHistoryDto(DailyOperationSizingHistory history,
                                              OperatorDocument operatorDocument,
                                              string sizingBeamNumber,
                                              string shiftName)
        {
            Id = history.Identity;
            SizingBeamNumber = sizingBeamNumber;
            DateTimeMachine = history.DateTimeMachine;
            ShiftName = shiftName;
            OperatorName = operatorDocument.CoreAccount.Name;
            OperatorGroup = operatorDocument.Group;
            MachineStatus = history.MachineStatus;
            BrokenPerShift = history.BrokenPerShift;
        }
    }
}
