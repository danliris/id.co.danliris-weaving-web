using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.ReadModels
{
    public class WeavingEstimatedProductionReadModel : ReadModelBase
    {
        public WeavingEstimatedProductionReadModel(Guid identity) : base(identity)
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


    }
}