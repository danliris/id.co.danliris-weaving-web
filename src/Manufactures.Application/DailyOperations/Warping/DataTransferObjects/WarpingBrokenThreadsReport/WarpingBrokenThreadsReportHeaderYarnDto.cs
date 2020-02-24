using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportHeaderYarnDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "BrokenName")]
        public string BrokenName { get; set; }

        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        public WarpingBrokenThreadsReportHeaderYarnDto(string supplierName,
                                                       string brokenName,
                                                       string yarnName)
        {
            SupplierName = supplierName;
            BrokenName = brokenName;
            YarnName = yarnName;
        }
    }
}
