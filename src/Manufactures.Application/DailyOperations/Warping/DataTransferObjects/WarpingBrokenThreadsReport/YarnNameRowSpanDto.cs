using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport
{
    public class YarnNameRowSpanDto
    {
        [JsonProperty(PropertyName = "SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty(PropertyName = "Span")]
        public int Span { get; set; }

        public YarnNameRowSpanDto(string supplierName, int span)
        {
            SupplierName = supplierName;
            Span = span;
        }
    }
}
