using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportFooterDto
    {
        [JsonProperty(PropertyName = "TotalBrokenValue")]
        public List<double> TotalBrokenValue { get; set; }

        [JsonProperty(PropertyName = "MaxBrokenValue")]
        public List<double> MaxBrokenValue { get; set; }

        [JsonProperty(PropertyName = "MinBrokenValue")]
        public List<double> MinBrokenValue { get; set; }

        [JsonProperty(PropertyName = "LastMonthAverageBrokenValue")]
        public List<double> LastMonthAverageBrokenValue { get; set; }

        public WarpingBrokenThreadsReportFooterDto(List<double> totalBrokenValue, 
                                                   List<double> maxBrokenValue, 
                                                   List<double> minBrokenValue) 
                                                   //List<double> lastMonthAverageBrokenValue)
        {
            TotalBrokenValue = totalBrokenValue;
            MaxBrokenValue = maxBrokenValue;
            MinBrokenValue = minBrokenValue;
            //LastMonthAverageBrokenValue = lastMonthAverageBrokenValue;
        }
    }
}
