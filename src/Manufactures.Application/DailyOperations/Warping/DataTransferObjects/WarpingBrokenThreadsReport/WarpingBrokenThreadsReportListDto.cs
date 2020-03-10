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

        [JsonProperty(PropertyName = "LastMonth")]
        public string LastMonth { get; set; }

        [JsonProperty(PropertyName = "Year")]
        public string Year { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitName")]
        public string WeavingUnitName { get; set; }

        [JsonProperty(PropertyName = "Items")]
        public List<Dictionary<string, string>> Items { get; set; }

        //[JsonProperty(PropertyName = "HeaderBrokens")]
        //public List<WarpingBrokenThreadsReportHeaderBrokenDto> HeaderBrokens { get; set; }

        //[JsonProperty(PropertyName = "BodyBrokensValue")]
        //public List<WarpingBrokenThreadsReportMappedBodyBrokenDto> BodyBrokensValue { get; set; }

        public WarpingBrokenThreadsReportListDto(string month, 
                                                 string lastMonth,
                                                 string year, 
                                                 string weavingUnitName,
                                                 List<Dictionary<string, string>> items)
                                                 //List<WarpingBrokenThreadsReportHeaderBrokenDto> headerBrokens,
                                                 //List<WarpingBrokenThreadsReportMappedBodyBrokenDto> bodyBrokensValue)
        {
            Month = month;
            LastMonth = lastMonth;
            Year = year;
            WeavingUnitName = weavingUnitName;
            Items = items;
            //HeaderBrokens = headerBrokens;
            //BodyBrokensValue = bodyBrokensValue;
        }

        public WarpingBrokenThreadsReportListDto()
        {
            Items = new List<Dictionary<string, string>>();
            //HeaderBrokens = new List<WarpingBrokenThreadsReportHeaderBrokenDto>();
            //BodyBrokensValue = new List<WarpingBrokenThreadsReportMappedBodyBrokenDto>();
        }
    }
}
