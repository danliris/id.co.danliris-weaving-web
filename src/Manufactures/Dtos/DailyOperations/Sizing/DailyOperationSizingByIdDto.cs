using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.Beams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public UnitId WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "WarpingBeamsDocument")]
        public List<BeamDto> WarpingBeamsDocument { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        //[JsonProperty(PropertyName = "Counter")]
        //public DailyOperationSizingCounterDto Counter { get; }

        //[JsonProperty(PropertyName = "Visco")]
        //public double Visco { get; }

        //[JsonProperty(PropertyName = "PIS")]
        //public int PIS { get; }

        //[JsonProperty(PropertyName = "SizingBeamDocumentId")]
        //public BeamId SizingBeamDocumentId { get; }

        [JsonProperty(PropertyName = "Details")]
        public List<DailyOperationSizingDetailsDto> Details { get; set; }

        public DailyOperationSizingByIdDto(DailyOperationSizingDocument document, string machineNumber, string constructionNumber, List<BeamDto> beams)
        {
            Id = document.Identity;
            MachineNumber = machineNumber;
            WeavingUnitDocumentId = document.WeavingUnitId;
            ConstructionNumber = constructionNumber;
            WarpingBeamsDocument = beams;
            Details = new List<DailyOperationSizingDetailsDto>();
        }
    }
}
