using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Entities
{
    public class DailyOperationMachineReaching : AggregateRoot<DailyOperationMachineReaching, DailyOperationMachineReachingReadModel>
    {
        protected override DailyOperationMachineReaching GetEntity()
        {
            return this;
        }
        public DailyOperationMachineReaching(Guid identity) : base(identity)
        {
        }

        public DailyOperationMachineReaching(Guid identity, int date, string month, int monthId, string yearPeriode, string year, 
            string shift, string group, string @operator, string mCNo, string code, string beamNo, string reachingInstall, 
            string installEfficiency, string cM, string beamWidth, string totalWarp, string reachingStrands, 
            string reachingEfficiency, string combWidth, string combStrands, string combEfficiency, string doffing, 
            string doffingEfficiency, string webbing, string margin, string combNo, string reedSpace, string eff2, 
            string checker, string information) : base(identity)
        {
            Date = date;
            Month = month;
            MonthId = monthId;
            YearPeriode = yearPeriode;
            Year = year;
            Shift = shift;
            Group = group;
            Operator = @operator;
            MCNo = mCNo;
            Code = code;
            BeamNo = beamNo;
            ReachingInstall = reachingInstall;
            InstallEfficiency = installEfficiency;
            CM = cM;
            BeamWidth = beamWidth;
            TotalWarp = totalWarp;
            ReachingStrands = reachingStrands;
            ReachingEfficiency = reachingEfficiency;
            CombWidth = combWidth;
            CombStrands = combStrands;
            CombEfficiency = combEfficiency;
            Doffing = doffing;
            DoffingEfficiency = doffingEfficiency;
            Webbing = webbing;
            Margin = margin;
            CombNo = combNo;
            ReedSpace = reedSpace;
            Eff2 = eff2;
            Checker = checker;
            Information = information;
            MarkTransient();

            ReadModel = new DailyOperationMachineReachingReadModel(Identity)
            {
                Date = this.Date,
                Month = this.Month,
                MonthId = this.MonthId,
                Year = this.Year,
                YearPeriode = this.YearPeriode,
                Shift = this.Shift,
                MCNo = this.MCNo,
                BeamNo= this.BeamNo,
                BeamWidth=this.BeamWidth,
                Checker=this.Checker,
                CM=this.CM,
                Code=this.Code,
                CombEfficiency=this.CombEfficiency,
                CombNo=this.CombNo,
                CombStrands=this.CombStrands,
                CombWidth=this.CombWidth,
                Doffing=this.Doffing,
                DoffingEfficiency=this.DoffingEfficiency,
                Eff2=this.Eff2,
                Information= this.Information,
                InstallEfficiency=this.InstallEfficiency,
                Margin=this.Margin,
                Operator=this.Operator,
                ReachingEfficiency=this.ReachingEfficiency,
                ReachingInstall=this.ReachingInstall,
                ReachingStrands=this.ReachingStrands,
                ReedSpace=this.ReedSpace,
                TotalWarp=this.TotalWarp,
                Webbing=this.Webbing,
                Group=this.Group
            };
        }

        public DailyOperationMachineReaching(DailyOperationMachineReachingReadModel readModel) : base(readModel)
        {
            this.Date = readModel.Date;
            this.Month = readModel.Month;
            this.Year = readModel.Year;
            this.YearPeriode = readModel.YearPeriode;
            this.Shift = readModel.Shift;
            this.MCNo = readModel.MCNo;
            this.Group = readModel.Group;
            this.Webbing = readModel.Webbing;
            this.TotalWarp = readModel.TotalWarp;
            this.BeamWidth = readModel.BeamWidth;
            this.Checker = readModel.Checker;
            this.CM = readModel.CM;
            this.Group = readModel.Group;
            this.Code = readModel.Code;
            this.BeamNo = readModel.BeamNo;
            this.CombEfficiency = readModel.CombEfficiency;
            this.CombNo = readModel.CombNo;
            this.CombStrands = readModel.CombStrands;
            this.CombWidth = readModel.CombWidth;
            this.DoffingEfficiency = readModel.DoffingEfficiency;
            this.Doffing = readModel.Doffing;
            this.Eff2 = readModel.Eff2;
            this.Information = readModel.Information;
            this.InstallEfficiency = readModel.InstallEfficiency;
            this.Margin = readModel.Margin;
            this.Operator = readModel.Operator;
            this.MonthId = readModel.MonthId;
            this.ReachingEfficiency = readModel.ReachingEfficiency;
            this.ReachingInstall = readModel.ReachingInstall;
            this.ReachingStrands = readModel.ReachingStrands;
            this.ReedSpace = readModel.ReedSpace;
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
