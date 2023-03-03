using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenGroupedReportQueryDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; set; }

        [JsonProperty(PropertyName = "BeamPerOperatorUom")]
        public int BeamPerOperatorUom { get; set; }

        [JsonProperty(PropertyName = "TotalBeamLength")]
        public double TotalBeamLength { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; set; }

        [JsonProperty(PropertyName = "TotalBrokenCause")]
        public int TotalBrokenCause { get; set; }

        public WarpingBrokenGroupedReportQueryDto(string supplierName,
                                                  string yarnName,
                                                  int beamPerOperatorUom,
                                                  double totalBeamLength,
                                                  int amountOfCones,
                                                  int totalBrokenCause)
        {
            SupplierName = supplierName;
            YarnName = yarnName;
            BeamPerOperatorUom = beamPerOperatorUom;
            TotalBeamLength = totalBeamLength;
            AmountOfCones = amountOfCones;
            TotalBrokenCause = totalBrokenCause;
        }
    }
}
