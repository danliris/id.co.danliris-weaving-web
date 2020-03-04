using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportBodyBrokenDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "ListOfYarn")]
        public List<WarpingBrokenThreadsReportListOfYarnDto> ListOfYarn { get; set; }

        [JsonProperty(PropertyName = "YarnNameRowSpan")]
        public int YarnNameRowSpan { get; set; }

        public WarpingBrokenThreadsReportBodyBrokenDto(string supplierName,
                                                       int yarnNameRowSpan
                                                       )
        {
            SupplierName = supplierName;
            YarnNameRowSpan = yarnNameRowSpan;
        }

        public WarpingBrokenThreadsReportBodyBrokenDto()
        {
            ListOfYarn = new List<WarpingBrokenThreadsReportListOfYarnDto>();
        }
    }
}
