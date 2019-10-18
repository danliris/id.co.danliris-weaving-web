using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects
{
    public class DailyOperationSizingBeamsWarpingDto : DailyOperationWarpingBeamDto
    {
        [JsonProperty(PropertyName = "WarpingBeamNumber")]
        public string WarpingBeamNumber { get; set; }

        public DailyOperationSizingBeamsWarpingDto(DailyOperationWarpingBeamDto warpingBeamDto, string warpingBeamNumber) : base(warpingBeamDto)
        {
            Id = warpingBeamDto.Id;
            WarpingBeamConeAmount = warpingBeamDto.WarpingBeamConeAmount;
            WarpingBeamNumber = warpingBeamNumber;
        }
    }
}
