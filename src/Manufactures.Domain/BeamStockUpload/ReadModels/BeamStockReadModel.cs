using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BeamStockUpload.ReadModels
{
    public class BeamStockReadModel : ReadModelBase
    {
        public BeamStockReadModel(Guid identity) : base(identity)
        {
        }

        public int Date { get; internal set; }
        public string YearPeriode { get; internal set; }
        public string MonthPeriode { get; internal set; }
        public int MonthPeriodeId { get; internal set; }
        public string Shift { get; internal set; }
        public string Beam { get; internal set; }
        public string Code { get; internal set; }
        public string Sizing { get; internal set; }
        public string InReaching { get; internal set; }
        public string Reaching { get; internal set; }
        public string Information { get; internal set; }
    }
}
