using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class DailyOperationWarpingBeamDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "WarpingBeamConeAmount")]
        public int WarpingBeamConeAmount { get; }

        public DailyOperationWarpingBeamDto(Guid id, int warpingBeamConeAmount)
        {
            Id = id;
            WarpingBeamConeAmount = warpingBeamConeAmount;
        }
    }
}
