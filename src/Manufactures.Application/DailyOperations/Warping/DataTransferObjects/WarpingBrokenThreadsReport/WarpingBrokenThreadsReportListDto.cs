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

        [JsonProperty(PropertyName = "HeaderSuppliers")]
        public List<WarpingBrokenThreadsReportHeaderSupplierDto> HeaderSuppliers { get; set; }

        [JsonProperty(PropertyName = "HeaderBrokens")]
        public List<WarpingBrokenThreadsReportHeaderBrokenDto> HeaderBrokens { get; set; }

        [JsonProperty(PropertyName = "BodyBrokens")]
        public List<WarpingBrokenThreadsReportBodyBrokenDto> BodyBrokensValue { get; set; }
        
        public WarpingBrokenThreadsReportListDto(string month, 
                                                 string year, 
                                                 string weavingUnitName,
                                                 List<WarpingBrokenThreadsReportHeaderSupplierDto> headerSuppliers,
                                                 List<WarpingBrokenThreadsReportHeaderBrokenDto> headerBrokens,
                                                 List<WarpingBrokenThreadsReportBodyBrokenDto> bodyBrokens)
        {
            Month = month;
            Year = year;
            WeavingUnitName = weavingUnitName;
            HeaderSuppliers = headerSuppliers;
            HeaderBrokens = headerBrokens;
            BodyBrokensValue = bodyBrokens;
        }

        public WarpingBrokenThreadsReportListDto()
        {
            HeaderSuppliers = new List<WarpingBrokenThreadsReportHeaderSupplierDto>();
            HeaderBrokens = new List<WarpingBrokenThreadsReportHeaderBrokenDto>();
            BodyBrokensValue = new List<WarpingBrokenThreadsReportBodyBrokenDto>();
        }
    }
}
