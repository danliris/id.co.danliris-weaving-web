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

        [JsonProperty(PropertyName = "TotalItems")]
        public int TotalItems { get; set; }

        [JsonProperty(PropertyName = "GroupedItems")]
        public List<WarpingBrokenThreadsGroupedItemsDto> GroupedItems { get; set; }

        public WarpingBrokenThreadsReportListDto(string month, 
                                                 string lastMonth,
                                                 string year, 
                                                 string weavingUnitName,
                                                 int totalItems,
                                                 List<WarpingBrokenThreadsGroupedItemsDto> groupedItems)
        {
            Month = month;
            LastMonth = lastMonth;
            Year = year;
            WeavingUnitName = weavingUnitName;
            TotalItems = totalItems;
            GroupedItems = groupedItems;
        }

        public WarpingBrokenThreadsReportListDto()
        {
            GroupedItems = new List<WarpingBrokenThreadsGroupedItemsDto>();
        }
    }
}
