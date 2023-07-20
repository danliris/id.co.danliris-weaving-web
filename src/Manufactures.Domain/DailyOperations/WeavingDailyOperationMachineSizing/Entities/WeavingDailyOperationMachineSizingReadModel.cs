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
        //public string DeletedDate { get; internal set; }
        //public string DeletedBy { get; internal set; }
        public int PeriodeId { get; internal set; }
        public string Periode { get; internal set; }
        public string Year { get; internal set; }
        public int Date { get; internal set; }
        public string Week { get; internal set; }
        public string MachineSizing { get; internal set; }
        public string Shift { get; internal set; }
        public string Group { get; internal set; }
        public string Lot { get; internal set; }
        public string SP { get; internal set; }
        public string YearProduction { get; internal set; }
        public string SPYear { get; internal set; }
        public string WarpType { get; internal set; }
        public string AL { get; internal set; }
        public string Construction { get; internal set; }
        public string Code { get; internal set; }
        public string ThreadOrigin { get; internal set; }
        public string Recipe { get; internal set; }
        public string Water { get; internal set; }
        public string BeamNo { get; internal set; }
        public string BeamWidth { get; internal set; }
        public string TekSQ { get; internal set; }
        public string ThreadCount { get; internal set; }
        public string Ne { get; internal set; }
        public string TempSD1 { get; internal set; }
        public string TempSD2 { get; internal set; }
        public string VisCoseSD1 { get; internal set; }
        public string VisCoseSD2 { get; internal set; }
        public string F1 { get; internal set; }
        public string F2 { get; internal set; }
        public string FDS { get; internal set; }
        public string FW { get; internal set; }
        public string FP { get; internal set; }
        public string A12 { get; internal set; }
        public string A34 { get; internal set; }
        public string B12 { get; internal set; }
        public string B34 { get; internal set; }
        public string C1234 { get; internal set; }
        public string Pis { get; internal set; }
        public string AddedLength { get; internal set; }
        public string Length { get; internal set; }
        public string EmptyBeamWeight { get; internal set; }
        public string Bruto { get; internal set; }
        public string Netto { get; internal set; }
        public string Teoritis { get; internal set; }
        public string SPU { get; internal set; }
        public string WarpingLenght { get; internal set; }
        public string FinalCounter { get; internal set; }
        public string Draft { get; internal set; }
        public string Speed { get; internal set; }
        public string Information { get; internal set; }
        public string SpeedMin { get; internal set; }
        public string Capacity { get; internal set; }
        public string Efficiency { get; internal set; }



    }
}