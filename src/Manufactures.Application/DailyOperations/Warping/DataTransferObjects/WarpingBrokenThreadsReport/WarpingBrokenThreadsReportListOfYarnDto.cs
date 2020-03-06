using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportListOfYarnDto
    {
        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        [JsonProperty(PropertyName = "BrokenEachYarn")]
        public List<double> BrokenEachYarn { get; set; }

        [JsonProperty(PropertyName = "TotalAllBroken")]
        public double TotalAllBroken { get; set; }

        [JsonProperty(PropertyName = "MaxBroken")]
        public double MaxBroken { get; set; }

        [JsonProperty(PropertyName = "MinBroken")]
        public double MinBroken { get; set; }

        [JsonProperty(PropertyName = "LastMonthAverageBroken")]
        public double LastMonthAverageBroken { get; set; }

        public WarpingBrokenThreadsReportListOfYarnDto(string yarnName, 
                                                       List<double> brokenEachYarn, 
                                                       double totalAllBroken, 
                                                       double maxBroken, 
                                                       double minBroken, 
                                                       double lastMonthAverageBroken)
        {
            YarnName = yarnName;
            BrokenEachYarn = brokenEachYarn;
            TotalAllBroken = totalAllBroken;
            MaxBroken = maxBroken;
            MinBroken = minBroken;
            LastMonthAverageBroken = lastMonthAverageBroken;
        }

        public WarpingBrokenThreadsReportListOfYarnDto()
        {
            BrokenEachYarn = new List<double>();
        }
    }
}
