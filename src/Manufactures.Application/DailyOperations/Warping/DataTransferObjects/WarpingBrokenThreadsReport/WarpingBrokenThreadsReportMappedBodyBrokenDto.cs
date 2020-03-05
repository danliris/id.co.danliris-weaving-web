using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportMappedBodyBrokenDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "ListOfYarn")]
        public List<WarpingBrokenThreadsReportListOfYarnDto> ListOfYarn { get; set; }

        [JsonProperty(PropertyName = "YarnNameRowSpan")]
        public int YarnNameRowSpan { get; set; }

        public WarpingBrokenThreadsReportMappedBodyBrokenDto(string supplierName, List<WarpingBrokenThreadsReportListOfYarnDto> listOfYarn, int yarnNameRowSpan)
        {
            SupplierName = supplierName;
            ListOfYarn = listOfYarn;
            YarnNameRowSpan = yarnNameRowSpan;
        }

        public WarpingBrokenThreadsReportMappedBodyBrokenDto()
        {
            ListOfYarn = new List<WarpingBrokenThreadsReportListOfYarnDto>();
        }
    }
}
