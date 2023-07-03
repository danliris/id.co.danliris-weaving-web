using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.ReadModels
{
    public class DailyOperationMachineReachingReadModel : ReadModelBase
    {
        public DailyOperationMachineReachingReadModel(Guid identity) : base(identity)
        {
        }

        public int Date { get; internal set; }
        public string Month { get; internal set; }
        public int MonthId { get; internal set; }
        public string YearPeriode { get; internal set; }
        public string Year { get; internal set; }
        public string Shift { get; internal set; }
        public string Group { get; internal set; }
        public string Operator { get; internal set; }
        public string MCNo { get; internal set; }
        public string Code { get; internal set; }
        public string BeamNo { get; internal set; }

        public string ReachingInstall { get; internal set; }
        public string InstallEfficiency { get; internal set; }

        public string CM { get; internal set; }
        public string BeamWidth { get; internal set; }
        public string TotalWarp { get; internal set; }

        public string ReachingStrands { get; internal set; }
        public string ReachingEfficiency { get; internal set; }

        public string CombWidth { get; internal set; }
        public string CombStrands { get; internal set; }
        public string CombEfficiency { get; internal set; }
        public string Doffing { get; internal set; }
        public string DoffingEfficiency { get; internal set; }
        public string Webbing { get; internal set; }
        public string Margin { get; internal set; }
        public string CombNo { get; internal set; }
        public string ReedSpace { get; internal set; }
        public string Eff2 { get; internal set; }
        public string Checker { get; internal set; }
        public string Information { get; internal set; }
    }
}
