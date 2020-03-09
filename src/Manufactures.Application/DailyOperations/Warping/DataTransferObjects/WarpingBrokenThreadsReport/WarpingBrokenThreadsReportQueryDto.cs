using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportQueryDto
    {
        [JsonProperty(PropertyName = "DateTimeProductBeam")]
        public DateTimeOffset? DateTimeProductBeam { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public int WeavingUnitId { get; set; }

        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "BrokenCauseName")]
        public string BrokenCauseName { get; set; }

        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        [JsonProperty(PropertyName = "YarnId")]
        public Guid YarnId { get; set; }

        [JsonProperty(PropertyName = "BeamLength")]
        public double BeamLength { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; set; }

        [JsonProperty(PropertyName = "BrokenEachYarn")]
        public int BrokenEachYarn { get; set; }

        [JsonProperty(PropertyName = "BeamProductUomId")]
        public int BeamProductUomId { get; set; }

        [JsonProperty(PropertyName = "BeamProductUomUnit")]
        public string BeamProductUomUnit { get; set; }

        public WarpingBrokenThreadsReportQueryDto(DateTimeOffset? dateTimeProductBeam, 
                                           int weavingUnitId, 
                                           string brokenCauseName, 
                                           string supplierName, 
                                           string yarnName,
                                           Guid yarnId,
                                           double beamLength,
                                           int amountOfCones, 
                                           int brokenEachYarn,
                                           int beamProductUomId,
                                           string beamProductUomUnit)
        {
            DateTimeProductBeam = dateTimeProductBeam;
            WeavingUnitId = weavingUnitId;
            BrokenCauseName = brokenCauseName;
            SupplierName = supplierName;
            YarnName = yarnName;
            YarnId = yarnId;
            BeamLength = beamLength;
            AmountOfCones = amountOfCones;
            BrokenEachYarn = brokenEachYarn;
            BeamProductUomId = beamProductUomId;
            BeamProductUomUnit = beamProductUomUnit;
        }
    }
}
