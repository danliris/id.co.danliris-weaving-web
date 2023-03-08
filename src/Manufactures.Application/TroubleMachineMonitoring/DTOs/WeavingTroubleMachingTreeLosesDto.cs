using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.TroubleMachineMonitoring.DTOs
{
    public class WeavingTroubleMachingTreeLosesDto
    {
        [JsonProperty(PropertyName = "Month")]
        public string Month { get; set; }
        [JsonProperty(PropertyName = "Year")]
        public string Year { get; set; }
        [JsonProperty(PropertyName = "YearPeriode")]
        public string YearPeriode { get; set; }
        [JsonProperty(PropertyName = "Group")]
        public string Group { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate { get; set; }
    }
}
