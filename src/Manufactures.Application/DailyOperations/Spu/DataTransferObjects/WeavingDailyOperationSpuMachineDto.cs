using Manufactures.Domain.DailyOperations.Spu.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Spu.DataTransferObjects
{
    public class WeavingDailyOperationSpuMachineDto 
    {
        
        public Guid Identity { get; set; }
        [JsonProperty(PropertyName = "Month")]
        public string Month {get; set; }
        [JsonProperty(PropertyName = "Year")]
        public string Year {get; set; }
        [JsonProperty(PropertyName = "YearPeriode")]
        public string YearPeriode { get; set; }
        [JsonProperty(PropertyName = "Group")]
        public string Group {get; set; }
        [JsonProperty(PropertyName = "MCNo")]
        public string MCNo {get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name {get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate { get; set; }
        [JsonProperty(PropertyName = "ThreadCut")]
        public double ThreadCut { get; set; }

        [JsonProperty(PropertyName = "Length")]
        public double Length { get; set; }
        [JsonProperty(PropertyName = "Date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get; set; }
        [JsonProperty(PropertyName = "Efficiency")]
        public string Efficiency { get; set; }

       

    }
}
