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

        [JsonProperty(PropertyName = "ProductionDate")]
        public DateTimeOffset ProductionDate { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public UnitId WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; }

        [JsonProperty(PropertyName = "PIS")]
        public int PIS { get; }

        public DailyOperationSizingListDto(DailyOperationSizingDocument document, DailyOperationSizingDetail details)
        {
            Id = document.Identity;
            ProductionDate = document.ProductionDate;
            WeavingUnitDocumentId = document.WeavingUnitId;
            MachineDocumentId = document.MachineDocumentId;
            ShiftDocumentId = new ShiftId(details.ShiftDocumentId.Value);
            BeamDocumentId = new BeamId(details.BeamDocumentId.Value);
            ConstructionDocumentId = new ConstructionId(details.ConstructionDocumentId.Value);
            PIS = details.PIS;
        }
    }
}
