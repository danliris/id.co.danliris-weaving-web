using Infrastructure.Domain;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.ReadModels
{
    public class WeavingEstimatedProduction : AggregateRoot<WeavingEstimatedProduction, WeavingEstimatedProductionReadModel>
    {
        public WeavingEstimatedProduction(Guid identity) : base(identity)
        {
        }
        public int Date { get; internal set; }
        public string Month { get; internal set; }
        public int MonthId { get; internal set; }
        public string YearPeriode { get; internal set; }
        public string YearSP { get; internal set; }
        public string SPNo { get; internal set; }
        public string Plait { get; internal set; }
        public float WarpLength { get; internal set; }
        public float Weft { get; internal set; }
        public float Width { get; internal set; }
        public string WarpType { get; internal set; }
        public string WeftType1 { get; internal set; }
        public string WeftType2 { get; internal set; }
        public string AL { get; internal set; }
        public string AP1 { get; internal set; }
        public string AP2 { get; internal set; }
        public string Thread { get; internal set; }
        public string Construction1 { get; internal set; }
        public string Buyer { get; internal set; }
        public float NumberOrder { get; internal set; }
        public string Construction2 { get; internal set; }
        public string WarpXWeft { get; internal set; }
        public float GradeA { get; internal set; }
        public float GradeB { get; internal set; }
        public float GradeC { get; internal set; }
        public float Total { get; internal set; }
        public float WarpBale { get; internal set; }
        public float WeftBale { get; internal set; }
        public float TotalBale { get; internal set; }

        WeavingEstimatedProduction(Guid identity, int Date,
        string Month,
        int MonthId,
        string YearPeriode,
        string YearSP,
        string SPNo,
        string Plait,
        float WarpLength,
        float Weft,
        float Width,
        string WarpType,
        string WeftType1,
        string WeftType2,
        string AL,
        string AP1,
        string AP2,
        string Thread,
        string Construction1,
        string Buyer,
        float NumberOrder,
        string Construction2,
        string WarpXWeft,
        float GradeA,
        float GradeB,
        float GradeC,
        float Total,
        float WarpBale,
        float WeftBale,
        float TotalBale) : base(identity)
        {
            this.Date = Date;
            this.Month = Month;
            this.MonthId = MonthId;
            this.YearPeriode = YearPeriode;
            this.YearSP = YearSP;
            this.SPNo = SPNo;
            this.Plait = Plait;
            this.WarpLength = WarpLength;
            this.Weft = Weft;
            this.Width = Width;
            this.WarpType = WarpType;
            this.WeftType1 = WeftType1;
            this.WeftType2 = WeftType2;
            this.AL = AL;
            this.AP1 = AP1;
            this.AP2 = AP2;
            this.Thread = Thread;
            this.Construction1 = Construction1;
            this.Buyer = Buyer;
            this.NumberOrder = NumberOrder;
            this.Construction2 = Construction2;
            this.WarpXWeft = WarpXWeft;
            this.GradeA = GradeA;
            this.GradeB = GradeB;
            this.GradeC = GradeC;
            this.Total = Total;
            this.WarpBale = WarpBale;
            this.WeftBale = WeftBale;
            this.TotalBale = TotalBale;
            MarkTransient();

            ReadModel = new WeavingEstimatedProductionReadModel(Identity)
            {
                Date = this.Date,
                Month = this.Month,
                MonthId = this.MonthId,
                YearPeriode = this.YearPeriode,
                YearSP = this.YearSP,
                SPNo = this.SPNo,
                Plait = this.Plait,
                WarpLength = this.WarpLength,
                Weft = this.Weft,
                Width = this.Width,
                WarpType = this.WarpType,
                WeftType1 = this.WeftType1,
                WeftType2 = this.WeftType2,
                AL = this.AL,
                AP1 = this.AP1,
                AP2 = this.AP2,
                Thread = this.Thread,
                Construction1 = this.Construction1,
                Buyer = this.Buyer,
                NumberOrder = this.NumberOrder,
                Construction2 = this.Construction2,
                WarpXWeft = this.WarpXWeft,
                GradeA = this.GradeA,
                GradeB = this.GradeB,
                GradeC = this.GradeC,
                Total = this.Total,
                WarpBale = this.WarpBale,
                WeftBale = this.WeftBale,
                TotalBale = this.TotalBale,
            };
        }

        protected override WeavingEstimatedProduction GetEntity()
        {
            return this;
        }
    }
}