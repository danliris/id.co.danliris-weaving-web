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

        [JsonProperty(PropertyName = "BrokenEachYarn")]
        public double BrokenEachYarn { get; set; }

        public WarpingListBrokenDto(string supplierName, string yarnName, double brokenEachYarn)
        {
            SupplierName = supplierName;
            YarnName = yarnName;
            BrokenEachYarn = brokenEachYarn;
        }
    }
}
