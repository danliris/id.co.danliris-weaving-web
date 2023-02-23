using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsGroupedItemsDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "ItemsValue")]
        public List<Dictionary<string, string>> ItemsValue { get; set; }

        [JsonProperty(PropertyName = "ItemsValueLength")]
        public int ItemsValueLength { get; set; }
    }
}
