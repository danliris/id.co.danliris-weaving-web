using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportHeaderBrokenDto
    {
        [JsonProperty(PropertyName = "BrokenName")]
        public string BrokenName { get; set; }

        public WarpingBrokenThreadsReportHeaderBrokenDto(string brokenName)
        {
            BrokenName = brokenName;
        }
    }
}
