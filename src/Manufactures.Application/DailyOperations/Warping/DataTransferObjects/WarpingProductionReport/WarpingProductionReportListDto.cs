using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingProductionReportListDto
    {
        [JsonProperty(PropertyName = "ProductionDate")]
        public int ProductionDate { get; set; }

        [JsonProperty(PropertyName = "AGroup")]
        public int AGroup { get; set; }

        [JsonProperty(PropertyName = "BGroup")]
        public int BGroup { get; set; }

        [JsonProperty(PropertyName = "CGroup")]
        public int CGroup { get; set; }

        [JsonProperty(PropertyName = "DGroup")]
        public int DGroup { get; set; }

        [JsonProperty(PropertyName = "EGroup")]
        public int EGroup { get; set; }

        [JsonProperty(PropertyName = "FGroup")]
        public int FGroup { get; set; }

        [JsonProperty(PropertyName = "GGroup")]
        public int GGroup { get; set; }

        [JsonProperty(PropertyName = "Total")]
        public int Total { get; set; }

        public WarpingProductionReportListDto(int productionDate, int aGroup, int bGroup, int cGroup, int dGroup, int eGroup, int fGroup, int gGroup, int total)
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
