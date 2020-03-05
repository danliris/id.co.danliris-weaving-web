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

        [JsonProperty(PropertyName = "HeaderBrokens")]
        public List<WarpingBrokenThreadsReportHeaderBrokenDto> HeaderBrokens { get; set; }

        [JsonProperty(PropertyName = "BodyBrokensValue")]
        public List<WarpingBrokenThreadsReportMappedBodyBrokenDto> BodyBrokensValue { get; set; }
        
        public WarpingBrokenThreadsReportListDto(string month, 
                                                 string year, 
                                                 string weavingUnitName,
                                                 List<WarpingBrokenThreadsReportHeaderBrokenDto> headerBrokens,
                                                 List<WarpingBrokenThreadsReportMappedBodyBrokenDto> bodyBrokensValue)
        {
            Month = month;
            Year = year;
            WeavingUnitName = weavingUnitName;
            HeaderBrokens = headerBrokens;
            BodyBrokensValue = bodyBrokensValue;
        }

        public WarpingBrokenThreadsReportListDto()
        {
            HeaderBrokens = new List<WarpingBrokenThreadsReportHeaderBrokenDto>();
            BodyBrokensValue = new List<WarpingBrokenThreadsReportMappedBodyBrokenDto>();
        }
    }
}
