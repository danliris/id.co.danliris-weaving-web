using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class PerOperatorProductionListDto
    {
        [JsonProperty(PropertyName = "ProductionDate")]
        public int ProductionDate { get; set; }

        [JsonProperty(PropertyName = "AGroup")]
        public double? AGroup { get; set; }

        [JsonProperty(PropertyName = "BGroup")]
        public double? BGroup { get; set; }

        [JsonProperty(PropertyName = "CGroup")]
        public double? CGroup { get; set; }

        [JsonProperty(PropertyName = "DGroup")]
        public double? DGroup { get; set; }

        [JsonProperty(PropertyName = "EGroup")]
        public double? EGroup { get; set; }

        [JsonProperty(PropertyName = "FGroup")]
        public double? FGroup { get; set; }

        [JsonProperty(PropertyName = "GGroup")]
        public double? GGroup { get; set; }

        [JsonProperty(PropertyName = "Total")]
        public double Total { get; set; }

        public PerOperatorProductionListDto(int productionDate, double aGroup, double bGroup, double cGroup, double dGroup, double eGroup, double fGroup, double gGroup, double total)
        {
            ProductionDate = productionDate;
            AGroup = aGroup;
            BGroup = bGroup;
            CGroup = cGroup;
            DGroup = dGroup;
            EGroup = eGroup;
            FGroup = fGroup;
            GGroup = gGroup;
            Total = total;
        }
    }
}
