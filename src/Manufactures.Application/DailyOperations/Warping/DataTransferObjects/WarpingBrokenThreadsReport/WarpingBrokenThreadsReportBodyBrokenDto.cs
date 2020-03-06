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

        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        [JsonProperty(PropertyName = "BrokenEachYarn")]
        public List<double> BrokenEachYarn { get; set; }

        [JsonProperty(PropertyName = "TotalAllBroken")]
        public double TotalAllBroken { get; set; }

        [JsonProperty(PropertyName = "MaxBroken")]
        public double MaxBroken { get; set; }

        [JsonProperty(PropertyName = "MinBroken")]
        public double MinBroken { get; set; }

        [JsonProperty(PropertyName = "LastMonthAverageBroken")]
        public double LastMonthAverageBroken { get; set; }

        public WarpingBrokenThreadsReportBodyBrokenDto(string supplierName, 
                                                       string yarnName, 
                                                       List<double> brokenEachYarn, 
                                                       double totalAllBroken, 
                                                       double maxBroken, 
                                                       double minBroken, 
                                                       double lastMonthAverageBroken)
        {
            SupplierName = supplierName;
            YarnName = yarnName;
            BrokenEachYarn = brokenEachYarn;
            TotalAllBroken = totalAllBroken;
            MaxBroken = maxBroken;
            MinBroken = minBroken;
            LastMonthAverageBroken = lastMonthAverageBroken;
        }

        public WarpingBrokenThreadsReportBodyBrokenDto()
        {
            BrokenEachYarn = new List<double>();
        }

        //[JsonProperty(PropertyName = "ListOfYarn")]
        //public List<WarpingBrokenThreadsReportListOfYarnDto> ListOfYarn { get; set; }

        //[JsonProperty(PropertyName = "YarnNameRowSpan")]
        //public int YarnNameRowSpan { get; set; }

        //public WarpingBrokenThreadsReportBodyBrokenDto(string supplierName,
        //                                               int yarnNameRowSpan
        //                                               )
        //{
        //    SupplierName = supplierName;
        //    YarnNameRowSpan = yarnNameRowSpan;
        //}

        //public WarpingBrokenThreadsReportBodyBrokenDto()
        //{
        //    ListOfYarn = new List<WarpingBrokenThreadsReportListOfYarnDto>();
        //}
    }
}
