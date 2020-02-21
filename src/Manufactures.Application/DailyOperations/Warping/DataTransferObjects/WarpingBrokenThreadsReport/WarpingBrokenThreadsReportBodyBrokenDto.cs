using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportBodyBrokenDto
    {
        [JsonProperty(PropertyName = "BrokenName")]
        public string BrokenName { get; set; }

        [JsonProperty(PropertyName = "ListBroken")]
        public List<WarpingListBrokenDto> ListBroken { get; set; }

        public WarpingBrokenThreadsReportBodyBrokenDto(string brokenName, List<WarpingListBrokenDto> listBroken)
        {
            BrokenName = brokenName;
            ListBroken = listBroken;
        }

        public WarpingBrokenThreadsReportBodyBrokenDto()
        {
            ListBroken = new List<WarpingListBrokenDto>();
        }
    }
}
