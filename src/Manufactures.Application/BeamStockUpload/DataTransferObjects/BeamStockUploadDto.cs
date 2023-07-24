using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.BeamStockUpload.DataTransferObjects
{
    public class BeamStockUploadDto
    {
        public Guid Identity { get; set; }
        [JsonProperty(PropertyName = "Date")]
        public int Date { get;  set; }
        [JsonProperty(PropertyName = "YearPeriode")]
        public string YearPeriode { get;  set; }
        [JsonProperty(PropertyName = "MonthPeriode")]
        public string MonthPeriode { get;  set; }
        [JsonProperty(PropertyName = "MonthPeriodeId")]
        public int MonthPeriodeId { get;  set; }
        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get;  set; }
        [JsonProperty(PropertyName = "Beam")]
        public string Beam { get;  set; }
        [JsonProperty(PropertyName = "Code")]
        public string Code { get;  set; }
        [JsonProperty(PropertyName = "Sizing")]
        public string Sizing { get;  set; }
        [JsonProperty(PropertyName = "InReaching")]
        public string InReaching { get;  set; }
        [JsonProperty(PropertyName = "Reaching")]
        public string Reaching { get;  set; }
        [JsonProperty(PropertyName = "Information")]
        public string Information { get;  set; }

        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate { get; set; }

        [JsonProperty(PropertyName = "Periode")]
        public DateTime Periode { get; set; }

        [JsonProperty(PropertyName = "UploadDate")]
        public DateTimeOffset UploadDate { get; set; }
    }
}
