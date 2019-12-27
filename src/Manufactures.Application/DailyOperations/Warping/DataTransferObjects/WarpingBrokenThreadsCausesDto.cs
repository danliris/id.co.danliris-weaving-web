using Manufactures.Domain.DailyOperations.Warping.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class WarpingBrokenThreadsCausesDto
    {
        [JsonProperty(PropertyName = "WarpingBrokenCauseName")]
        public string WarpingBrokenCauseName { get; set; }

        [JsonProperty(PropertyName = "TotalBroken")]
        public int TotalBroken { get; set; }

        public WarpingBrokenThreadsCausesDto(DailyOperationWarpingBrokenCause brokenCauseDocument, string warpingBrokenCauseName)
        {
            WarpingBrokenCauseName = warpingBrokenCauseName;
            TotalBroken = brokenCauseDocument.TotalBroken;
        }
    }
}
