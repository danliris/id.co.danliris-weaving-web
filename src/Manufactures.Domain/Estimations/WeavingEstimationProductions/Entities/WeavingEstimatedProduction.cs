using Infrastructure.Domain;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.ReadModels
{
    public class WeavingEstimatedProduction : AggregateRoot<WeavingEstimatedProduction, WeavingEstimatedProductionReadModel>
    {
        private int v1;
        private string v2;
        private string v3;
        private string v4;
        private string v5;
        private double? v6;
        private double? v7;
        private double? v8;
        private string v9;
        private string v10;
        private string v11;
        private string v12;
        private string v13;
        private string v14;
        private string v15;
        private string v16;
        private string v17;
        private double v18;
        private string v19;
        private string v20;
        private double v21;
        private double v22;
        private double v23;
        private double v24;
        private double v25;
        private double v26;
        private double v27;

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
        public double WarpLength { get; internal set; }
        public double Weft { get; internal set; }
        public double Width { get; internal set; }
        public string WarpType { get; internal set; }
        public string WeftType1 { get; internal set; }
        public string WeftType2 { get; internal set; }
        public string AL { get; internal set; }
        public string AP1 { get; internal set; }
        public string AP2 { get; internal set; }
        public string Thread { get; internal set; }
        public string Construction1 { get; internal set; }
        public string Buyer { get; internal set; }
        public double NumberOrder { get; internal set; }
        public string Construction2 { get; internal set; }
        public string WarpXWeft { get; internal set; }
        public double GradeA { get; internal set; }
        public double GradeB { get; internal set; }
        public double GradeC { get; internal set; }
        public double Aval { get; internal set; }
        public double Total { get; internal set; }
        public double WarpBale { get; internal set; }
        public double WeftBale { get; internal set; }
        public double TotalBale { get; internal set; }

        public WeavingEstimatedProduction(Guid identity, int Date,
        string Month,
        int MonthId,
        string YearPeriode,
        string YearSP,
        string SPNo,
        string Plait,
        double WarpLength,
        double Weft,
        double Width,
        string WarpType,
        string WeftType1,
        string WeftType2,
        string AL,
        string AP1,
        string AP2,
        string Thread,
        string Construction1,
        string Buyer,
        double NumberOrder,
        string Construction2,
        string WarpXWeft,
        double GradeA,
        double GradeB,
        double GradeC,
        double Aval,
        double Total,
        double WarpBale,
        double WeftBale,
        double TotalBale) : base(identity)
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
            this.Aval = Aval;
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
                Aval = this.Aval,
                Total = this.Total,
                WarpBale = this.WarpBale,
                WeftBale = this.WeftBale,
                TotalBale = this.TotalBale
            };
        }

        public WeavingEstimatedProduction(WeavingEstimatedProductionReadModel readModel) : base(readModel)
        {
            this.Date = readModel.Date;
            this.Month = readModel.Month;
            this.MonthId = readModel.MonthId;
            this.YearPeriode = readModel.YearPeriode;
            this.YearSP = readModel.YearSP;
            this.SPNo = readModel.SPNo;
            this.Plait = readModel.Plait;
            this.WarpLength = readModel.WarpLength;
            this.Weft = readModel.Weft;
            this.Width = readModel.Width;
            this.WarpType = readModel.WarpType;
            this.WeftType1 = readModel.WeftType1;
            this.WeftType2 = readModel.WeftType2;
            this.AL = readModel.AL;
            this.AP1 = readModel.AP1;
            this.AP2 = readModel.AP2;
            this.Thread = readModel.Thread;
            this.Construction1 = readModel.Construction1;
            this.Buyer = readModel.Buyer;
            this.NumberOrder = readModel.NumberOrder;
            this.Construction2 = readModel.Construction2;
            this.WarpXWeft = readModel.WarpXWeft;
            this.GradeA = readModel.GradeA;
            this.GradeB = readModel.GradeB;
            this.GradeC = readModel.GradeC;
            this.Aval = readModel.Aval;
            this.Total = readModel.Total;
            this.WarpBale = readModel.WarpBale;
            this.WeftBale = readModel.WeftBale;
            this.TotalBale = readModel.TotalBale;

        }

         protected override WeavingEstimatedProduction GetEntity()
        {
            return this;
        }
    }
}