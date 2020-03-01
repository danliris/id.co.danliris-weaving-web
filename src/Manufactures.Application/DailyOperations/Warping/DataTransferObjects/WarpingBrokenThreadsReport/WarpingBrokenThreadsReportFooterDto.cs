using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportFooterDto
    {
        [JsonProperty(PropertyName = "LastMonth")]
        public string LastMonth { get; set; }

        [JsonProperty(PropertyName = "TotalBrokenValue")]
        public List<WarpingBrokenThreadsReportFooterTotalDto> TotalBrokenValue { get; set; }

        [JsonProperty(PropertyName = "MaxBrokenValue")]
        public List<double> MaxBrokenValue { get; set; }

        [JsonProperty(PropertyName = "MinBrokenValue")]
        public List<double> MinBrokenValue { get; set; }

        [JsonProperty(PropertyName = "LastMonthAverageBrokenValue")]
        public List<WarpingBrokenThreadsReportFooterTotalDto> LastMonthAverageBrokenValue { get; set; }

        public WarpingBrokenThreadsReportFooterDto(string lastMonth,
                                                   List<WarpingBrokenThreadsReportFooterTotalDto> totalBrokenValue, 
                                                   List<double> maxBrokenValue, 
                                                   List<double> minBrokenValue,
                                                   List<WarpingBrokenThreadsReportFooterTotalDto> lastMonthAverageBrokenValue)
        {
            LastMonth = lastMonth;
            TotalBrokenValue = totalBrokenValue;
            MaxBrokenValue = maxBrokenValue;
            MinBrokenValue = minBrokenValue;
            LastMonthAverageBrokenValue = lastMonthAverageBrokenValue;
        }

        public WarpingBrokenThreadsReportFooterDto()
        {
            TotalBrokenValue = new List<WarpingBrokenThreadsReportFooterTotalDto>();
            MaxBrokenValue = new List<double>();
            MinBrokenValue = new List<double>();
            LastMonthAverageBrokenValue = new List<WarpingBrokenThreadsReportFooterTotalDto>();
        }
    }
}
