using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.TroubleMachineMonitoring.DTOs
{
    public class WeavingTroubleMachingTreeLosesDto
    {
        public int Date { get;  set; }
        public string Month { get;  set; }
        public int MonthId { get;  set; }
        public string YearPeriode { get;  set; }
        public string Shift { get;  set; }
        public string Description { get;  set; }
        public string WarpingMachineNo { get;  set; }
        public string Group { get;  set; }
        public string Code { get;  set; }
        public string DownTimeMC { get;  set; }
        public double TimePerMinutes { get;  set; }
        public string  Start { get;  set; }
        public string  Finish { get; set; }
        public string CreatedDate { get; set; }

    }
}
