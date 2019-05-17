using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public UnitId WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "WarpingBeamCollectionDocumentId")]
        public List<BeamId> WarpingBeamCollectionDocumentId { get; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingCounterDto Counter { get; }

        [JsonProperty(PropertyName = "Visco")]
        public double Visco { get; }

        [JsonProperty(PropertyName = "PIS")]
        public int PIS { get; }

        [JsonProperty(PropertyName = "SizingBeamDocumentId")]
        public BeamId SizingBeamDocumentId { get; }

        [JsonProperty(PropertyName = "DailyOperationSizingDetails")]
        public List<DailyOperationSizingDetailsDto> DailyOperationSizingDetails { get; }

        public DailyOperationSizingByIdDto(DailyOperationSizingDocument document)
        {
            Id = document.Identity;
            MachineDocumentId = document.MachineDocumentId;
            WeavingUnitDocumentId = document.WeavingUnitId;
            WarpingBeamCollectionDocumentId = document.WarpingBeamCollectionDocumentId.Deserialize<List<BeamId>>();
            ConstructionDocumentId = document.ConstructionDocumentId;
            Counter = document.Counter.Deserialize<DailyOperationSizingCounterDto>();
            Visco = document.Visco;
            PIS = document.PIS;
            SizingBeamDocumentId = document.SizingBeamDocumentId;
            foreach (var details in document.DailyOperationSizingDetails)
            {
                var detailsDto = new DailyOperationSizingDetailsDto(details);
                DailyOperationSizingDetails.Add(detailsDto);
            }
        }
    }
}
