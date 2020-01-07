using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingProductionReportListDto
    {
        [JsonProperty(PropertyName = "AGroupTotal")]
        public double? AGroupTotal { get; set; }

        [JsonProperty(PropertyName = "BGroupTotal")]
        public double? BGroupTotal { get; set; }

        [JsonProperty(PropertyName = "CGroupTotal")]
        public double? CGroupTotal { get; set; }

        [JsonProperty(PropertyName = "DGroupTotal")]
        public double? DGroupTotal { get; set; }

        [JsonProperty(PropertyName = "EGroupTotal")]
        public double? EGroupTotal { get; set; }

        [JsonProperty(PropertyName = "FGroupTotal")]
        public double? FGroupTotal { get; set; }

        [JsonProperty(PropertyName = "GGroupTotal")]
        public double? GGroupTotal { get; set; }

        [JsonProperty(PropertyName = "TotalAll")]
        public double TotalAll { get; set; }

        [JsonProperty(PropertyName = "PerOperatorList")]
        public List<PerOperatorProductionListDto> PerOperatorList { get; set; }

        public WarpingProductionReportListDto(double aGroupTotal, double bGroupTotal, double cGroupTotal, double dGroupTotal, double eGroupTotal, double fGroupTotal, double gGroupTotal, double totalAll, List<PerOperatorProductionListDto> perOperatorList)
        {
            AGroupTotal = aGroupTotal;
            BGroupTotal = bGroupTotal;
            CGroupTotal = cGroupTotal;
            DGroupTotal = dGroupTotal;
            EGroupTotal = eGroupTotal;
            FGroupTotal = fGroupTotal;
            GGroupTotal = gGroupTotal;
            TotalAll = totalAll;
            PerOperatorList = perOperatorList;
        }

        public WarpingProductionReportListDto()
        {
        }
    }
}
