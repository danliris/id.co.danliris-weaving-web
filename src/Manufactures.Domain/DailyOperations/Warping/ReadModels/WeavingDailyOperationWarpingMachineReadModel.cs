using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class WeavingDailyOperationWarpingMachineReadModel : ReadModelBase
    {
        public WeavingDailyOperationWarpingMachineReadModel(Guid identity) : base(identity)
        {
            
        }
        public int Date { get; internal set; }
        public string Month { get; internal set; }
        public string YearPeriode { get; internal set; }
        public string Year { get; internal set; }
        public string Shift { get; internal set; }
        public string MCNo { get; internal set; }
        public string Name { get; internal set; }
        public string Group { get; internal set; }
        public string Lot { get; internal set; }
        public string SP { get; internal set; }
        public string YearSP { get; internal set; }
        public string WarpType { get; internal set; }
        public string AL { get; internal set; }
        public string Construction { get; internal set; }
        public string Code { get; internal set; }
        public string BeamNo { get; internal set; }
        public int TotalCone { get; internal set; }
        public string ThreadNo { get; internal set; }
        public double Length { get; internal set; }
        public string Uom { get; internal set; }
        public DateTime Start { get; internal set; }
        public DateTime Doff { get; internal set; }
        public double HNLeft { get; internal set; }
        public double HNMiddle { get; internal set; }
        public double HNRight { get; internal set; }
        public double SpeedMeterPerMinute { get; internal set; }
        public double ThreadCut { get; internal set; }
        public double Capacity { get; internal set; }
        public string Eff { get; internal set; }

    }
}
