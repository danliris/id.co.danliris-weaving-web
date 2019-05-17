using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
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

        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public UnitId WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; }

        [JsonProperty(PropertyName = "PIS")]
        public int PIS { get; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; }

        public DailyOperationSizingListDto(DailyOperationSizingDocument document, DailyOperationSizingDetail details)
        {
            Id = document.Identity;
            MachineDocumentId = document.MachineDocumentId;
            WeavingUnitDocumentId = document.WeavingUnitId;
            ConstructionDocumentId = new ConstructionId(document.ConstructionDocumentId.Value);
            PIS = document.PIS;
            ShiftDocumentId = new ShiftId(details.ShiftDocumentId);
        }
    }
}
