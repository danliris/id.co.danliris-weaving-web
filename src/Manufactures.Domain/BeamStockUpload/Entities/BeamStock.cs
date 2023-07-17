using Infrastructure.Domain;
using Manufactures.Domain.BeamStockUpload.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BeamStockUpload.Entities
{
    public class BeamStock : AggregateRoot<BeamStock, BeamStockReadModel>
    {
        protected override BeamStock GetEntity()
        {
            return this;
        }
        public BeamStock(Guid identity) : base(identity)
        {
        }

        public BeamStock(Guid identity, int date, string yearPeriode, string monthPeriode, int monthPeriodeId, string shift,
            string beam, string code, string sizing, string inReaching, string reaching, string information) : base(identity)
        {
            Date = date;
            YearPeriode = yearPeriode;
            MonthPeriode = monthPeriode;
            MonthPeriodeId = monthPeriodeId;
            Shift = shift;
            Beam = beam;
            Code = code;
            Sizing = sizing;
            InReaching = inReaching;
            Reaching = reaching;
            Information = information;
            MarkTransient();

            ReadModel = new BeamStockReadModel(Identity)
            {
                Date = this.Date,
                MonthPeriode = this.MonthPeriode,
                MonthPeriodeId = this.MonthPeriodeId,
                YearPeriode = this.YearPeriode,
                Shift=this.Shift,
                Beam=this.Beam,
                Code=this.Code,
                Sizing=this.Sizing,
                InReaching=this.InReaching,
                Reaching=this.Reaching,
                Information=this.Information
            };
        }

        public BeamStock(BeamStockReadModel readModel) : base(readModel)
        {
            Date = readModel.Date;
            MonthPeriode = readModel.MonthPeriode;
            MonthPeriodeId = readModel.MonthPeriodeId;
            YearPeriode = readModel.YearPeriode;
            Shift = readModel.Shift;
            Beam = readModel.Beam;
            Code = readModel.Code;
            Sizing = readModel.Sizing;
            InReaching = readModel.InReaching;
            Reaching = readModel.Reaching;
            Information = readModel.Information;
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
