using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingProductionReportListDto
    {
        [JsonProperty(PropertyName = "Month")]
        public string Month { get; set; }

        [JsonProperty(PropertyName = "Year")]
        public string Year { get; set; }

        [JsonProperty(PropertyName = "Headers")]
        public List<WarpingProductionReportHeaderDto> Headers { get; set; }

        [JsonProperty(PropertyName = "Groups")]
        public List<WarpingProductionReportGroupDto> Groups { get; set; }

        [JsonProperty(PropertyName = "ProcessedList")]
        public List<WarpingProductionReportProcessedListDto> ProcessedList { get; set; }

        public WarpingProductionReportListDto(string month, string year, List<WarpingProductionReportHeaderDto> headers, List<WarpingProductionReportGroupDto> groups, List<WarpingProductionReportProcessedListDto> processedList)
        {
            Month = month;
            Year = year;
            Headers = headers;
            Groups = groups;
            ProcessedList = processedList;
        }

        public WarpingProductionReportListDto()
        {
            Headers = new List<WarpingProductionReportHeaderDto>();
            Groups = new List<WarpingProductionReportGroupDto>();
            ProcessedList = new List<WarpingProductionReportProcessedListDto>();
        }
    }
}
