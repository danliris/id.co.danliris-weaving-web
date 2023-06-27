using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Productions.ReadModels
{
    public class WeavingDailyOperationMachineSizingReadModel : ReadModelBase
    {
        public WeavingDailyOperationMachineSizingReadModel(Guid identity) : base(identity)
        {

        }
        //public int Identity { get; internal set; }
        //public string RowVersion { get; internal set; }
        //public int CreatedBy { get; internal set; }
        //public string CreatedDate { get; internal set; }
        //public string ModifiedBy { get; internal set; }
        //public string ModifiedDate { get; internal set; }
        //public string Deleted { get; internal set; }
        //public double DeletedDate { get; internal set; }
        //public double DeletedBy { get; internal set; }
        public double PeriodeId { get; internal set; }
        public string Periode { get; internal set; }
        public string Year { get; internal set; }
        public int Date { get; internal set; }
        public int Week { get; internal set; }
        public string MachineSizing { get; internal set; }
        public string Shift { get; internal set; }
        public string Group { get; internal set; }
        public string Lot { get; internal set; }
        public double SP { get; internal set; }
        public string YearProduction { get; internal set; }
        public string SPYear { get; internal set; }
        public string WarpType { get; internal set; }
        public string AL { get; internal set; }
        public string Construction { get; internal set; }
        public string Code { get; internal set; }
        public string ThreadOrigin { get; internal set; }
        public string Recipe { get; internal set; }
        public double Water { get; internal set; }
        public string BeamNo { get; internal set; }
        public string BeamWidth { get; internal set; }
        public string TekSQ { get; internal set; }
        public double ThreadCount { get; internal set; }
        public double Ne { get; internal set; }
        public double TempSD1 { get; internal set; }
        public double TempSD2 { get; internal set; }
        public double VisCoseSD1 { get; internal set; }
        public double VisCoseSD2 { get; internal set; }
        public double F1 { get; internal set; }
        public double F2 { get; internal set; }
        public double FDS { get; internal set; }
        public double FW { get; internal set; }
        public double FP { get; internal set; }
        public double A12 { get; internal set; }
        public double A34 { get; internal set; }
        public string B12 { get; internal set; }
        public string B34 { get; internal set; }
        public double C1234 { get; internal set; }
        public string Pis { get; internal set; }
        public string AddedLength { get; internal set; }
        public double Length { get; internal set; }
        public double EmptyBeamWeight { get; internal set; }
        public double Bruto { get; internal set; }
        public double Netto { get; internal set; }
        public double Teoritis { get; internal set; }
        public string SPU { get; internal set; }
        public double WarpingLenght { get; internal set; }
        public double FinalCounter { get; internal set; }
        public string Draft { get; internal set; }
        public double Speed { get; internal set; }
        public string Information { get; internal set; }
        public double SpeedMin { get; internal set; }
        public double Capacity { get; internal set; }
        public string Efficiency { get; internal set; }



    }
}