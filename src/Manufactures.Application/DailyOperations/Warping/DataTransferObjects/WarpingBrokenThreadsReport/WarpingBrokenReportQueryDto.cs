using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenReportQueryDto
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

        [JsonProperty(PropertyName = "BeamPerOperatorUom")]
        public int BeamPerOperatorUom { get; set; }

        public WarpingBrokenReportQueryDto(DateTimeOffset? dateTimeProductBeam, 
                                           int weavingUnitId, 
                                           string brokenCauseName, 
                                           string supplierName, 
                                           string yarnName,
                                           Guid yarnId,
                                           double beamLength,
                                           int amountOfCones, 
                                           int brokenEachYarn, 
                                           int beamPerOperatorUom)
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
            BeamPerOperatorUom = beamPerOperatorUom;
        }
    }
}
