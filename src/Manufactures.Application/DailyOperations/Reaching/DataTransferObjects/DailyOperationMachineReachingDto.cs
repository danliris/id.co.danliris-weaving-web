using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationMachineReachingDto
    {
        public Guid Identity { get; set; }
        [JsonProperty(PropertyName = "Date")]
        public int Date { get;  set; }
        [JsonProperty(PropertyName = "Month")]
        public string Month { get;  set; }
        [JsonProperty(PropertyName = "MonthId")]
        public int MonthId { get;  set; }
        [JsonProperty(PropertyName = "YearPeriode")]
        public string YearPeriode { get;  set; }
        [JsonProperty(PropertyName = "Year")]
        public string Year { get;  set; }
        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get;  set; }
        [JsonProperty(PropertyName = "Group")]
        public string Group { get;  set; }
        [JsonProperty(PropertyName = "Operator")]
        public string Operator { get;  set; }
        [JsonProperty(PropertyName = "MCNo")]
        public string MCNo { get;  set; }
        [JsonProperty(PropertyName = "Code")]
        public string Code { get;  set; }
        [JsonProperty(PropertyName = "BeamNo")]
        public string BeamNo { get;  set; }

        [JsonProperty(PropertyName = "ReachingInstall")]
        public string ReachingInstall { get;  set; }
        [JsonProperty(PropertyName = "InstallEfficiency")]
        public string InstallEfficiency { get;  set; }

        [JsonProperty(PropertyName = "CM")]
        public string CM { get;  set; }
        [JsonProperty(PropertyName = "BeamWidth")]
        public string BeamWidth { get;  set; }
        [JsonProperty(PropertyName = "TotalWarp")]
        public string TotalWarp { get;  set; }

        [JsonProperty(PropertyName = "ReachingStrands")]
        public string ReachingStrands { get;  set; }
        [JsonProperty(PropertyName = "ReachingEfficiency")]
        public string ReachingEfficiency { get;  set; }

        [JsonProperty(PropertyName = "CombWidth")]
        public string CombWidth { get;  set; }
        [JsonProperty(PropertyName = "CombStrands")]
        public string CombStrands { get;  set; }
        [JsonProperty(PropertyName = "CombEfficiency")]
        public string CombEfficiency { get;  set; }
        [JsonProperty(PropertyName = "Doffing")]
        public string Doffing { get;  set; }
        [JsonProperty(PropertyName = "DoffingEfficiency")]
        public string DoffingEfficiency { get;  set; }
        [JsonProperty(PropertyName = "Webbing")]
        public string Webbing { get;  set; }
        [JsonProperty(PropertyName = "Margin")]
        public string Margin { get;  set; }
        [JsonProperty(PropertyName = "CombNo")]
        public string CombNo { get;  set; }
        [JsonProperty(PropertyName = "ReedSpace")]
        public string ReedSpace { get;  set; }
        [JsonProperty(PropertyName = "Eff2")]
        public string Eff2 { get;  set; }
        [JsonProperty(PropertyName = "Checker")]
        public string Checker { get;  set; }
        [JsonProperty(PropertyName = "Information")]
        public string Information { get;  set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate { get; set; }

        [JsonProperty(PropertyName = "Periode")]
        public DateTime Periode { get; set; }

        [JsonProperty(PropertyName = "Efficiency")]
        public decimal Efficiency { get; set; }
    }
}
