using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class DailyProcessedPerOperatorDto
    {
        [JsonProperty(PropertyName = "Group")]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Total")]
        public double Total { get; set; }

        public DailyProcessedPerOperatorDto(string group, string name, double total)
        {
            Group = group;
            Name = name;
            Total = total;
        }
    }
}
