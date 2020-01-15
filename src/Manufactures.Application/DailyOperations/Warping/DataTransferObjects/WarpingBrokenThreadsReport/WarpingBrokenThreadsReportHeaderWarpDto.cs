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

        [JsonProperty(PropertyName = "WarpName")]
        public int WarpName { get; set; }

        [JsonProperty(PropertyName = "Span")]
        public int Span { get; set; }

        public WarpingBrokenThreadsReportHeaderWarpDto(string spinningUnit, int warpName, int span)
        {
            SpinningUnit = spinningUnit;
            WarpName = warpName;
            Span = span;
        }
    }
}
