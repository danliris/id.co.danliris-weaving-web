using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class DailyOperationWarpingBeamDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamConeAmount")]
        public int WarpingBeamConeAmount { get; set; }

        public DailyOperationWarpingBeamDto(Guid id, int warpingBeamConeAmount)
        {
            Id = id;
            WarpingBeamConeAmount = warpingBeamConeAmount;
        }

        public DailyOperationWarpingBeamDto(DailyOperationWarpingBeamDto warpingBeamDto)
        {
            Id = warpingBeamDto.Id;
            WarpingBeamConeAmount = warpingBeamDto.WarpingBeamConeAmount;
        }
    }
}
