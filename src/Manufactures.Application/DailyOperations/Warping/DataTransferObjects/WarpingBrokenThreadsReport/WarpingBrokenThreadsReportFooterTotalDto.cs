using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportFooterTotalDto
    {

        [JsonProperty(PropertyName = "WarpName")]
        public string WarpName { get; set; }

        [JsonProperty(PropertyName = "TotalValue")]
        public double TotalValue { get; set; }

        public WarpingBrokenThreadsReportFooterTotalDto(string warpName, double totalValue)
        {
            WarpName = warpName;
            TotalValue = totalValue;
        }
    }
}
