using Manufactures.Domain.DailyOperations.Sizing;
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

        [JsonProperty(PropertyName = "WarpingBeamsDocument")]
        public List<BeamDto> WarpingBeamsDocument { get; }

        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; }

        [JsonProperty(PropertyName = "NeReal")]
        public double NeReal { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public int WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "SizingBeamDocuments")]
        public List<DailyOperationSizingBeamDocumentsDto> SizingBeamDocuments { get; set; }

        [JsonProperty(PropertyName = "SizingDetails")]
        public List<DailyOperationSizingDetailsDto> SizingDetails { get; set; }

        public DailyOperationSizingByIdDto(DailyOperationSizingDocument document, string machineNumber, string machineType, int weavingUnitDocumentId, string constructionNumber, List<BeamDto> beams, double emptyWeight, double yarnStrands, double neReal)
        {
            Id = document.Identity;
            MachineNumber = machineNumber;
            MachineType = machineType;
            WeavingUnitDocumentId = weavingUnitDocumentId;
            ConstructionNumber = constructionNumber;
            WarpingBeamsDocument = beams;
            EmptyWeight = emptyWeight;
            YarnStrands = yarnStrands;
            NeReal = neReal;
            SizingBeamDocuments = new List<DailyOperationSizingBeamDocumentsDto>();
            SizingDetails = new List<DailyOperationSizingDetailsDto>();

            //foreach (var sizingBeamDocument in document.SizingBeamDocuments)
            //{
            //    var sizingBeam = new DailyOperationSizingBeamDocumentDto(sizingBeamDocument.sizin, sizingBeamDocument.Start, sizingBeamDocument.Finish, sizingBeamDocument.Weight, sizingBeamDocument.Bruto, sizingBeamDocument.Theoritical, sizingBeamDocument.PISMeter, sizingBeamDocument.SPU, sizingBeamDocument.SizingBeamStatus);
            //    SizingBeamDocuments.Add(sizingBeam);
            //}
        }
    }
}
