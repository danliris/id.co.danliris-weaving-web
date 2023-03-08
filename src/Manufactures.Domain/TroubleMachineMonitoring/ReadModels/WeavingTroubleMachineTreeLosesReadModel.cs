using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.TroubleMachineMonitoring.ReadModels
{
    public class WeavingTroubleMachineTreeLosesReadModel : ReadModelBase
    {
        public WeavingTroubleMachineTreeLosesReadModel(Guid identity) : base(identity)
        {
        }
        public int Date { get; internal set; }
        public string Month { get; internal set; }
        public int MonthId { get; internal set; }
        public string YearPeriode { get; internal set; }
        public string Shift { get; internal set; }
        public string Description { get; internal set; }
        public string WarpingMachineNo { get; internal set; }
        public string Group { get; internal set; }
        public string Code { get; internal set; }
        public DateTime DownTimeMC { get; internal set; }
        public double TimePerMinutes { get; internal set; }
        public DateTime Start { get; internal set; }
        public DateTime Finish { get; internal set; }
        

    }
}
