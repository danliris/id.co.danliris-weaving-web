using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Domain.Beams;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.Beams
{
    public class BeamDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; }

        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; }

        [JsonProperty(PropertyName = "YarnLength")]
        public double YarnLength { get; private set; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; private set; }

        public BeamDto(BeamDocument document)
        {
            Id = document.Identity;
            Number = document.Number;
            Type = document.Type;
            EmptyWeight = document.EmptyWeight;
            YarnLength = document.YarnLength;
            YarnStrands = document.YarnStrands;
        }

        public BeamDto(DailyOperationWarpingBeamDto warpingBeam, BeamDocument beamDocument)
        {
            Id = warpingBeam.Id;
            YarnStrands = warpingBeam.WarpingBeamConeAmount;
            Number = beamDocument.Number;
            Type = beamDocument.Type;
            EmptyWeight = beamDocument.EmptyWeight;
            YarnLength = beamDocument.YarnLength;
        }

        public BeamDto(DailyOperationSizingBeamDto sizingBeam, BeamDocument beamDocument)
        {
            Id = sizingBeam.Id;
            Number = beamDocument.Number;
            Type = beamDocument.Type;
            EmptyWeight = beamDocument.EmptyWeight;
            YarnLength = sizingBeam.SizingLengthCounter;
        }
    }
}
