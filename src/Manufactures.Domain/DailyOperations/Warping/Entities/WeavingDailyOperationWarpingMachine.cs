using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class WeavingDailyOperationWarpingMachine : AggregateRoot<WeavingDailyOperationWarpingMachine, WeavingDailyOperationWarpingMachineReadModel>
    {
        public WeavingDailyOperationWarpingMachine(Guid identity) : base(identity)
        {
        }
        public int Date { get; internal set; }
        public string Month { get; internal set; }
        public int MonthId { get; internal set; }
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
        public int Week { get; internal set; }

        public WeavingDailyOperationWarpingMachine(Guid identity, int Date,
          string Month,
          int MonthId,
          string Year,
          string YearPeriode,
          string Shift,
          string MCNo,
          string Name,
          string Group,
          string Lot,
          string SP,
          string YearSP,
          string WarpType,
          string AL,
          string Construction,
          string Code,
          string BeamNo,
          int TotalCone,
          string ThreadNo,
          double Length,
          string Uom,
          DateTime Start,
          DateTime Doff,
          double HNLeft,
          double HNMiddle,
          double HNRight,
          double SpeedMeterPerMinute,
          double ThreadCut,
          double Capacity,
          string Eff,
          int Week) : base(identity)
        {
            this.Date = Date;
            this.Month = Month;
            this.MonthId = MonthId;
            this.Year = Year;
            this.YearPeriode = YearPeriode;
            this.Shift = Shift;
            this.MCNo = MCNo;
            this.Name = Name;
            this.Group = Group;
            this.Lot = Lot;
            this.SP = SP;
            this.YearSP = YearSP;
            this.WarpType = WarpType;
            this.AL = AL;
            this.Construction = Construction;
            this.Code = Code;
            this.BeamNo = BeamNo;
            this.TotalCone = TotalCone;
            this.ThreadNo = ThreadNo;
            this.Length = Length;
            this.Uom = Uom;
            this.Start = Start;
            this.Doff = Doff;
            this.HNLeft = HNLeft;
            this.HNMiddle = HNMiddle;
            this.HNRight = HNRight;
            this.SpeedMeterPerMinute = SpeedMeterPerMinute;
            this.ThreadCut = ThreadCut;
            this.Capacity = Capacity;
            this.Eff = Eff;
            this.Week = Week;
            MarkTransient();

            ReadModel = new WeavingDailyOperationWarpingMachineReadModel(Identity)
            {
                Date = this.Date,
                Month = this.Month,
                MonthId = this.MonthId,
                Year = this.Year,
                YearPeriode = this.YearPeriode,
                Shift = this.Shift,
                MCNo = this.MCNo,
                Name = this.Name,
                Group = this.Group,
                Lot = this.Lot,
                SP = this.SP,
                YearSP = this.YearSP,
                WarpType = this.WarpType,
                AL = this.AL,
                Construction = this.Construction,
                Code = this.Code,
                BeamNo = this.BeamNo,
                TotalCone = this.TotalCone,
                ThreadNo = this.ThreadNo,
                Length = this.Length,
                Uom = this.Uom,
                Start = this.Start,
                Doff = this.Doff,
                HNLeft = this.HNLeft,
                HNMiddle = this.HNMiddle,
                HNRight = this.HNRight,
                SpeedMeterPerMinute = this.SpeedMeterPerMinute,
                ThreadCut = this.ThreadCut,
                Capacity = this.Capacity,
                Eff= this.Eff,
                Week=this.Week
            };

        }
        public WeavingDailyOperationWarpingMachine(WeavingDailyOperationWarpingMachineReadModel readModel) : base(readModel)
        {

            this.Date = readModel.Date;
            this.Month = readModel.Month;
            this.Year = readModel.Year;
            this.YearPeriode = readModel.YearPeriode;
            this.Shift = readModel.Shift;
            this.MCNo = readModel.MCNo;
            this.Name = readModel.Name;
            this.Lot = readModel.Lot;
            this.SP = readModel.SP;
            this.YearSP = readModel.YearSP;
            this.WarpType = readModel.WarpType;
            this.AL = readModel.AL;
            this.Group = readModel.Group;
            this.Construction = readModel.Construction;
            this.Code = readModel.Code;
            this.BeamNo = readModel.BeamNo;
            this.TotalCone = readModel.TotalCone;
            this.ThreadNo = readModel.ThreadNo;
            this.Length = readModel.Length;
            this.Uom = readModel.Uom;
            this.Start = readModel.Start;
            this.Doff = readModel.Doff;
            this.HNLeft = readModel.HNLeft;
            this.HNMiddle = readModel.HNMiddle;
            this.HNRight = readModel.HNRight;
            this.SpeedMeterPerMinute = readModel.SpeedMeterPerMinute;
            this.ThreadCut = readModel.ThreadCut;
            this.Capacity = readModel.Capacity;
            this.Week = readModel.Week;
        }

        protected override WeavingDailyOperationWarpingMachine GetEntity()
        {
            return this;
        }
    }
}
