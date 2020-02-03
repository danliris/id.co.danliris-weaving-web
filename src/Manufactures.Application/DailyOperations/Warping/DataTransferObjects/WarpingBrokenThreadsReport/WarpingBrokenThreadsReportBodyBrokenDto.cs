using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportBodyBrokenDto
    {
        [JsonProperty(PropertyName = "BrokenName")]
        public string BrokenName { get; set; }

        //[JsonProperty(PropertyName = "SpinningUnit")]
        //public string SpinningUnit { get; set; }

        [JsonProperty(PropertyName = "WarpName")]
        public string WarpName { get; set; }

        [JsonProperty(PropertyName = "BrokenValue")]
        public double BrokenValue { get; set; }

        public WarpingBrokenThreadsReportBodyBrokenDto(string brokenName, 
                                                       //string spinningUnit, 
                                                       string warpName, 
                                                       double brokenValue)
        {
            BrokenName = brokenName;
            //SpinningUnit = spinningUnit;
            WarpName = warpName;
            BrokenValue = brokenValue;
        }

        public WarpingBrokenThreadsReportBodyBrokenDto()
        {
        }
    }
}
