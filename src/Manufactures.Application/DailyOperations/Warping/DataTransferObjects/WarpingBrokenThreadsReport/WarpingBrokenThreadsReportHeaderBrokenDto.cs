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

        [JsonProperty(PropertyName = "LastMonth")]
        public string LastMonth { get; set; }

        public WarpingBrokenThreadsReportHeaderBrokenDto(string brokenName, string lastMonth)
        {
            BrokenName = brokenName;
            LastMonth = lastMonth;
        }
    }
}
