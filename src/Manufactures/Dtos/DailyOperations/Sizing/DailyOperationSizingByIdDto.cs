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

        [JsonProperty(PropertyName = "MachineType")]
        public string MachineType { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public UnitId WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "WarpingBeamsDocument")]
        public List<BeamDto> WarpingBeamsDocument { get; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; }

        [JsonProperty(PropertyName = "NeReal")]
        public double NeReal { get;}

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "SizingBeamDocuments")]
        public List<DailyOperationSizingBeamDocumentsDto> SizingBeamDocuments { get; set; }

        [JsonProperty(PropertyName = "SizingDetails")]
        public List<DailyOperationSizingDetailsDto> SizingDetails { get; set; }

        public DailyOperationSizingByIdDto(DailyOperationSizingDocument document, string machineNumber, string machineType, string constructionNumber, List<BeamDto> beams, double yarnStrands, double neReal)
        {
            Id = document.Identity;
            MachineNumber = machineNumber;
            MachineType = machineType;
            WeavingUnitDocumentId = document.WeavingUnitId;
            ConstructionNumber = constructionNumber;
            WarpingBeamsDocument = beams;
            YarnStrands = yarnStrands;
            NeReal = neReal;
            SizingBeamDocuments = new List<DailyOperationSizingBeamDocumentsDto>();

            //foreach (var sizingBeamDocument in document.SizingBeamDocuments)
            //{
            //    var sizingBeam = new DailyOperationSizingBeamDocumentDto(sizingBeamDocument.sizin, sizingBeamDocument.Start, sizingBeamDocument.Finish, sizingBeamDocument.Weight, sizingBeamDocument.Bruto, sizingBeamDocument.Theoritical, sizingBeamDocument.PISMeter, sizingBeamDocument.SPU, sizingBeamDocument.SizingBeamStatus);
            //    SizingBeamDocuments.Add(sizingBeam);
            //}
            SizingDetails = new List<DailyOperationSizingDetailsDto>();
        }
    }
}
