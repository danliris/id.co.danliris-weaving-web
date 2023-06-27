using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Production.DataTransferObjects
{
    public class WeavingDailyOperationMachineSizingDto
    {
        [JsonProperty(PropertyName = "Month")]
        public string Month { get; set; }
        
        [JsonProperty(PropertyName = "YearPeriode")]
        public string YearPeriode { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
       
        public string CreatedDate { get; set; }
        [JsonProperty(PropertyName = "Date")]
        public int Date { get;  set; }
        [JsonProperty(PropertyName = "YearSP")]
        public string YearSP { get;  set; }
        public string SPNo { get;  set; }
        public string Plait { get;  set; }
        public double WarpLength { get;  set; }
        public double Weft { get;  set; }
        public double Width { get;  set; }
        public string WarpType { get;  set; }
        public string WeftType1 { get;  set; }
        public string WeftType2 { get;  set; }
        public string AL { get;  set; }
        public string AP1 { get;  set; }
        public string AP2 { get;  set; }
        public string Thread { get;  set; }
        public string Construction1 { get;  set; }
        public string Buyer { get;  set; }
        public double NumberOrder { get;  set; }
        public string Construction2 { get;  set; }
        public string WarpXWeft { get;  set; }
        public double GradeA { get;  set; }
        public double GradeB { get;  set; }
        public double GradeC { get;  set; }
        public double Aval { get;  set; }
        public double Total { get;  set; }
        public double WarpBale { get;  set; }
        public double WeftBale { get;  set; }
        public double TotalBale { get;  set; }


    }
}
