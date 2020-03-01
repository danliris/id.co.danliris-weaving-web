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

        [JsonProperty(PropertyName = "BrokenCauseName")]
        public string BrokenCauseName { get; set; }

        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        [JsonProperty(PropertyName = "BrokenEachCause")]
        public int BrokenEachCause { get; set; }

        [JsonProperty(PropertyName = "BeamPerOperatorUom")]
        public int BeamPerOperatorUom { get; set; }

        [JsonProperty(PropertyName = "TotalBeamLength")]
        public double TotalBeamLength { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; set; }

        public WarpingBrokenReportQueryDto(DateTimeOffset? dateTimeProductBeam, 
                                           int weavingUnitId, 
                                           string brokenCauseName, 
                                           string supplierName, 
                                           string yarnName, 
                                           int brokenEachCause, 
                                           int beamPerOperatorUom, 
                                           double totalBeamLength, 
                                           int amountOfCones)
        {
            DateTimeProductBeam = dateTimeProductBeam;
            WeavingUnitId = weavingUnitId;
            BrokenCauseName = brokenCauseName;
            SupplierName = supplierName;
            YarnName = yarnName;
            BrokenEachCause = brokenEachCause;
            BeamPerOperatorUom = beamPerOperatorUom;
            TotalBeamLength = totalBeamLength;
            AmountOfCones = amountOfCones;
        }
    }
}
