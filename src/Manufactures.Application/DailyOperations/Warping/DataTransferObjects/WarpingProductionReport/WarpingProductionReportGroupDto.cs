using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingProductionReportGroupDto
    {
        [JsonProperty(PropertyName = "Group")]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "Span")]
        public int Span { get; set; }

        public WarpingProductionReportGroupDto(string group, int span)
        {
            Group = group;
            Span = span;
        }
    }
}
