using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsGroupedItemsDto
    {
        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        //[JsonProperty(PropertyName = "YarnName")]
        //public string YarnName { get; set; }
    }
}
