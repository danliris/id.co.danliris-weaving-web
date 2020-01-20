using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportHeaderWarpDto
    {
        [JsonProperty(PropertyName = "SpinningUnit")]
        public string SpinningUnit { get; set; }

        [JsonProperty(PropertyName = "BrokenName")]
        public string BrokenName { get; set; }

        [JsonProperty(PropertyName = "WarpName")]
        public string WarpName { get; set; }

        public WarpingBrokenThreadsReportHeaderWarpDto(string spinningUnit,
                                                       string brokenName,
                                                       string warpName)
        {
            SpinningUnit = spinningUnit;
            BrokenName = brokenName;
            WarpName = warpName;
        }
    }
}
