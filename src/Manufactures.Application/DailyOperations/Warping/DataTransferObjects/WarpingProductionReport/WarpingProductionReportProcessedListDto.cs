using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingProductionReportProcessedListDto
    {
        [JsonProperty(PropertyName = "Day")]
        public int Day { get; set; }

        [JsonProperty(PropertyName = "DailyProcessedPerOperator")]
        public List<DailyProcessedPerOperatorDto> DailyProcessedPerOperator { get; set; }

        public WarpingProductionReportProcessedListDto(int day, List<DailyProcessedPerOperatorDto> dailyProcessedPerOperator)
        {
            Day = day;
            DailyProcessedPerOperator = dailyProcessedPerOperator;
        }
    }
}
