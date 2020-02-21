using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingListBrokenDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        [JsonProperty(PropertyName = "BrokenEach")]
        public double BrokenEach { get; set; }

        public WarpingListBrokenDto(string supplierName, string yarnName, double brokenEach)
        {
            SupplierName = supplierName;
            YarnName = yarnName;
            BrokenEach = brokenEach;
        }
    }
}
