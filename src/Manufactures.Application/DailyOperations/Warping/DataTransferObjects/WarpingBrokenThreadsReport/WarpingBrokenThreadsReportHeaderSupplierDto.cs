using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class WarpingBrokenThreadsReportHeaderSupplierDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "Span")]
        public int Span { get; set; }

        public WarpingBrokenThreadsReportHeaderSupplierDto(string supplierName,
                                                           int span)
        {
            SupplierName = supplierName;
            Span = span;
        }
    }
}
