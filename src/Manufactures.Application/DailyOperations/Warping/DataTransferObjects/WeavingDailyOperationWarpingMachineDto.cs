using Manufactures.Domain.DailyOperations.Warping.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class WeavingDailyOperationWarpingMachineDto 
    {
        
        public Guid Identity { get; set; }
        [JsonProperty(PropertyName = "Month")]
        public string Month {get; set; }
        [JsonProperty(PropertyName = "MonthId")]
        public int MonthId { get; set; }
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
        [JsonProperty(PropertyName = "Eff")]
        public decimal Eff { get; set; }
        [JsonProperty(PropertyName = "Week")]
        public int Week { get; set; }
        [JsonProperty(PropertyName = "Day")]
        public int Day { get; set; }

        // [JsonProperty(PropertyName = "Lot")]
        //public string Lot {get; set; }
        //[JsonProperty(PropertyName = "SP")]
        //public string SP { get; set; }
        //[JsonProperty(PropertyName = "YearSP")]
        //public string YearSP { get; set; }
        //[JsonProperty(PropertyName = "WarpType")]
        //public string WarpType { get; set; }
        //[JsonProperty(PropertyName = "Code")]
        //public string Code {get; set; }
        //[JsonProperty(PropertyName = "BeamNo")]
        //public string BeamNo {get; set; }
        //[JsonProperty(PropertyName = "TotalCone")]
        //public int TotalCone {get; set; }
        //[JsonProperty(PropertyName = "ThreadNo")]
        //public string ThreadNo { get; set; }
        //[JsonProperty(PropertyName = "Uom")]
        //public string Uom {get; set; }
        //[JsonProperty(PropertyName = "Start")]
        //public DateTime Start {get; set; }
        //[JsonProperty(PropertyName = "Doff")]
        //public DateTime Doff {get; set; }
        //[JsonProperty(PropertyName = "HNLeft")]
        //public double HNLeft {get; set; }
        //[JsonProperty(PropertyName = "HNMiddle")]
        //public double HNMiddle {get; set; }
        //[JsonProperty(PropertyName = "HNRight")]
        //public double HNRight {get; set; }
        //[JsonProperty(PropertyName = "SpeedMeterPerMinute")]
        //public double SpeedMeterPerMinute {get; set; }
        //[JsonProperty(PropertyName = "Capacity")]
        //public double Capacity {get; set; }
        //[JsonProperty(PropertyName = "Eff")]
        //public double Eff {get; set; }

        //public WeavingDailyOperationWarpingMachineDto(WeavingDailyOperationWarpingMachine weaving)
        //{

        //    // Date = weaving.Date;
        //    Month = weaving.Month;
        //    Year = weaving.Year;
        //    //Shift = weaving.Shift;
        //    MCNo = weaving.MCNo;
        //    Name = weaving.Name;
        //    YearPeriode = weaving.YearPeriode;
        //    Group = weaving.Group;
        //    //Lot = weaving.Lot;
        //    //SP = weaving.SP;
        //    //YearSP = weaving.YearSP;
        //    //WarpType = weaving.WarpType;
        //    //Code = weaving.Code;
        //    //BeamNo = weaving.BeamNo;
        //    //TotalCone = weaving.TotalCone;
        //    //ThreadNo = weaving.ThreadNo;
        //    //Length = weaving.Length;
        //    //Uom = weaving.Uom;
        //    //Start = weaving.Start;
        //    //Doff = weaving.Doff;
        //    //HNLeft = weaving.HNLeft;
        //    //HNMiddle = weaving.HNMiddle;
        //    //HNRight = weaving.HNRight;
        //    //SpeedMeterPerMinute = weaving.SpeedMeterPerMinute;
        //    //ThreadCut = weaving.ThreadCut;
        //    //Capacity = weaving.Capacity;
        //}

    }
}
