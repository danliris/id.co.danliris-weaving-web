using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportHeaderSupplierDto
    {
        [JsonProperty(PropertyName = "Unit")]
        public string Unit { get; set; }

        [JsonProperty(PropertyName = "Span")]
        public int Span { get; set; }

        public WarpingBrokenThreadsReportHeaderSupplierDto(string unit,
                                                           int span)
        {
            Unit = unit;
            Span = span;
        }
    }
}
