using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
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

        [JsonProperty(PropertyName = "SizingBeamDocument")]
        public List<DailyOperationSizingBeamDocumentDto> SizingBeamDocument { get; }

        [JsonProperty(PropertyName = "SizingDetails")]
        public List<DailyOperationSizingDetailsDto> SizingDetails { get; set; }

        public DailyOperationSizingByIdDto(DailyOperationSizingDocument document, string machineNumber, string constructionNumber, List<BeamDto> beams)
        {
            Id = document.Identity;
            MachineNumber = machineNumber;
            WeavingUnitDocumentId = document.WeavingUnitId;
            ConstructionNumber = constructionNumber;
            WarpingBeamsDocument = beams;
            SizingBeamDocument = new List<DailyOperationSizingBeamDocumentDto>();

            //foreach (var sizingBeamDocument in document.SizingBeamDocument)
            //{
            //    var sizingBeam = new DailyOperationSizingBeamDocumentDto(sizingBeamDocument.sizin, sizingBeamDocument.Start, sizingBeamDocument.Finish, sizingBeamDocument.Netto, sizingBeamDocument.Bruto, sizingBeamDocument.Theoritical, sizingBeamDocument.PISMeter, sizingBeamDocument.SPU, sizingBeamDocument.SizingBeamStatus);
            //    SizingBeamDocument.Add(sizingBeam);
            //}
            SizingDetails = new List<DailyOperationSizingDetailsDto>();
        }
    }
}
