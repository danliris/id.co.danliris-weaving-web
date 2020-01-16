using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportListDto
    {
        [JsonProperty(PropertyName = "Month")]
        public string Month { get; set; }

        [JsonProperty(PropertyName = "Year")]
        public string Year { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitName")]
        public string WeavingUnitName { get; set; }

        [JsonProperty(PropertyName = "HeaderWarps")]
        public List<WarpingBrokenThreadsReportHeaderWarpDto> HeaderWarps { get; set; }

        [JsonProperty(PropertyName = "BodyBrokens")]
        public List<WarpingBrokenThreadsReportBodyBrokenDto> BodyBrokens { get; set; }

        [JsonProperty(PropertyName = "Footers")]
        public WarpingBrokenThreadsReportFooterDto Footers { get; set; }

        public WarpingBrokenThreadsReportListDto(string month, 
                                                 string year, 
                                                 string weavingUnitName, 
                                                 List<WarpingBrokenThreadsReportHeaderWarpDto> headerWarps, 
                                                 List<WarpingBrokenThreadsReportBodyBrokenDto> bodyBrokens,
                                                 WarpingBrokenThreadsReportFooterDto footers)
        {
            Month = month;
            Year = year;
            WeavingUnitName = weavingUnitName;
            HeaderWarps = headerWarps;
            BodyBrokens = bodyBrokens;
            Footers = footers;
        }

        public WarpingBrokenThreadsReportListDto()
        {
            HeaderWarps = new List<WarpingBrokenThreadsReportHeaderWarpDto>();
            BodyBrokens = new List<WarpingBrokenThreadsReportBodyBrokenDto>();
        }
    }
}
