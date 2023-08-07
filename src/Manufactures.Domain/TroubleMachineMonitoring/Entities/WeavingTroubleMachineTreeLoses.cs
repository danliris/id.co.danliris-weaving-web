using Infrastructure.Domain;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.TroubleMachineMonitoring.Entities
{
    public class WeavingTroubleMachineTreeLoses : AggregateRoot<WeavingTroubleMachineTreeLoses, WeavingTroubleMachineTreeLosesReadModel>
    {
        public WeavingTroubleMachineTreeLoses(Guid identity) : base(identity)
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

        public int Week { get; internal set; }
        public WeavingTroubleMachineTreeLoses(Guid identity,
         int Date,
         string Month,
         int MonthId,
         string YearPeriode,
         string Shift,
         string Description,
         string WarpingMachineNo,
         string Group,
         string Code,
         DateTime DownTimeMC,
         double TimePerMinutes,
         DateTime Start,
         DateTime Finish, int Week) : base(identity)
        {
            this.Date = Date;
            this.Month = Month;
            this.MonthId = MonthId;
            this.YearPeriode = YearPeriode;
            this.Shift = Shift;
            this.Description = Description;
            this.WarpingMachineNo = WarpingMachineNo;
            this.Group = Group;
            this.Code = Code;
            this.DownTimeMC = DownTimeMC;
            this.TimePerMinutes = TimePerMinutes;
            this.Start = Start;
            this.Finish = Finish;
            this.Week = Week;
            MarkTransient();

            ReadModel = new WeavingTroubleMachineTreeLosesReadModel(Identity)
            {
                Date = this.Date,
                Month = this.Month,
                MonthId = this.MonthId,
                YearPeriode = this.YearPeriode,
                Shift = this.Shift,
                Description = this.Description,
                WarpingMachineNo = this.WarpingMachineNo,
                Group = this.Group,
                Code = this.Code,
                DownTimeMC = this.DownTimeMC,
                TimePerMinutes = this.TimePerMinutes,
                Start = this.Start,
                Finish = this.Finish,
                Week=this.Week
            };
        }
        public WeavingTroubleMachineTreeLoses(WeavingTroubleMachineTreeLosesReadModel readModel) :base (readModel)
        {
            this.Date = readModel.Date;
            this.Month = readModel.Month;
            this.MonthId = readModel.MonthId;
            this.YearPeriode = readModel.YearPeriode;
            this.Shift = readModel.Shift;
            this.Description = readModel.Description;
            this.WarpingMachineNo = readModel.WarpingMachineNo;
            this.Group = readModel.Group;
            this.Code = readModel.Code;
            this.DownTimeMC = readModel.DownTimeMC;
            this.TimePerMinutes = readModel.TimePerMinutes;
            this.Start = readModel.Start;
            this.Finish = readModel.Finish;
            this.Week = readModel.Week;
        }
        protected override WeavingTroubleMachineTreeLoses GetEntity()
        {
            return this;
        }

    }
}
