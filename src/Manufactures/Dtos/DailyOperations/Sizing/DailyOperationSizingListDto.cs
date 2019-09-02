using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public int WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; }

        [JsonProperty(PropertyName = "DateTimeMachineHistory")]
        public DateTimeOffset DateTimeMachineHistory { get; }

        public DailyOperationSizingListDto(DailyOperationSizingDocument document, MachineDocument machineDocument, int weavingUnitDocumentId, FabricConstructionDocument constructionDocument, ShiftValueObject shiftDocument, string operationStatus, DateTimeOffset dateTimeMachineHistory)
        {
            Id = document.Identity;
            MachineNumber = machineDocument.MachineNumber;
            WeavingUnitDocumentId = weavingUnitDocumentId;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            ShiftName = shiftDocument.Name;
            OperationStatus = operationStatus;
            DateTimeMachineHistory = dateTimeMachineHistory;
        }
    }
}
