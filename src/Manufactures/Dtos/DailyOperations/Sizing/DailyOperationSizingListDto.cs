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
        public UnitId WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "DateTimeOperationHistory")]
        public DateTimeOffset DateTimeOperationHistory { get; }

        public DailyOperationSizingListDto(DailyOperationSizingDocument document, MachineDocument machineDocument, FabricConstructionDocument constructionDocument, ShiftValueObject shiftDocument, DateTimeOffset dateTimeOperation)
        {
            Id = document.Identity;
            MachineNumber = machineDocument.MachineNumber;
            WeavingUnitDocumentId = document.WeavingUnitId;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            ShiftName = shiftDocument.Name;
            DateTimeOperationHistory = dateTimeOperation;
        }
    }
}
