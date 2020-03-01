using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportBodyBrokenDto
    {
        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        [JsonProperty(PropertyName = "BrokenName")]
        public string BrokenName { get; set; }

        [JsonProperty(PropertyName = "TotalEachBroken")]
        public double TotalEachBroken { get; set; }

        [JsonProperty(PropertyName = "LastMonthName")]
        public string LastMonthName { get; set; }

        [JsonProperty(PropertyName = "TotalAllBroken")]
        public double TotalAllBroken { get; set; }

        [JsonProperty(PropertyName = "MaxBroken")]
        public double MaxBroken { get; set; }

        [JsonProperty(PropertyName = "MinBroken")]
        public double MinBroken { get; set; }

        [JsonProperty(PropertyName = "LastMonthAverageBroken")]
        public double LastMonthAverageBroken { get; set; }

        public WarpingBrokenThreadsReportBodyBrokenDto(string yarnName,
                                                       string brokenName,
                                                       double totalEachBroken,
                                                       string lastMonthName,
                                                       double totalAllBroken,
                                                       double maxBroken,
                                                       double minBroken,
                                                       double lastMonthAverageBroken)
        {
            YarnName = yarnName;
            BrokenName = brokenName;
            TotalEachBroken = totalEachBroken;
            LastMonthName = lastMonthName;
            TotalAllBroken = totalAllBroken;
            MaxBroken = maxBroken;
            MinBroken = minBroken;
            LastMonthAverageBroken = lastMonthAverageBroken;
        }
    }
}
