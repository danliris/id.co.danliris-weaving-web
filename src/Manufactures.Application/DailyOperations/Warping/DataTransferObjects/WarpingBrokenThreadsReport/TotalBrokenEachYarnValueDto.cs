using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class TotalBrokenEachYarnValueDto
    {
        [JsonProperty(PropertyName = "TotalBrokenEachYarnValue")]
        public double TotalBrokenEachYarnValue { get; set; }

        public TotalBrokenEachYarnValueDto(double totalBrokenEachYarnValue)
        {
            TotalBrokenEachYarnValue = totalBrokenEachYarnValue;
        }
    }
}
