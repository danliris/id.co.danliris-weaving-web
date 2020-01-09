using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingProductionReportListDto
    {
        [JsonProperty(PropertyName = "Headers")]
        public List<WarpingProductionReportHeaderDto> Headers { get; set; }

        [JsonProperty(PropertyName = "ProcessedList")]
        public List<WarpingProductionReportProcessedListDto> ProcessedList { get; set; }

        public WarpingProductionReportListDto(List<WarpingProductionReportHeaderDto> headers, List<WarpingProductionReportProcessedListDto> processedList)
        {
            Headers = headers;
            ProcessedList = processedList;
        }

        public WarpingProductionReportListDto()
        {
        }
    }
}
