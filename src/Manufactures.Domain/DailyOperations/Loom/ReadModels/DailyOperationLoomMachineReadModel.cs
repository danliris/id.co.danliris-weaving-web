using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomMachineReadModel : ReadModelBase
    {
        public DailyOperationLoomMachineReadModel(Guid identity) : base(identity)
        {
        }

        public int Date { get; internal set; }
        public string YearPeriode { get; internal set; }
        public string MonthPeriode { get; internal set; }
        public int MonthPeriodeId { get; internal set; }
        public string Shift { get; internal set; }
        public string MCNo { get; internal set; }
        public string SPNo { get; internal set; }
        public string Year { get; internal set; }
        public string TA { get; internal set; }
        public string Warp { get; internal set; }
        public string Weft { get; internal set; }
        public string Length { get; internal set; }
        public string WarpType { get; internal set; }
        public string WeftType { get; internal set; }
        public string WeftType2 { get; internal set; }
        public string WeftType3 { get; internal set; }
        public string AL { get; internal set; }
        public string AP { get; internal set; }
        public string AP2 { get; internal set; }
        public string AP3 { get; internal set; }
        public string Thread { get; internal set; }
        public string Construction { get; internal set; }
        public string ThreadType { get; internal set; }
        public string MonthId { get; internal set; }
        public string ProductionCMPX { get; internal set; }
        public string EFFMC { get; internal set; }
        public string RPM { get; internal set; }
        public string T { get; internal set; }
        public string F { get; internal set; }
        public string W { get; internal set; }
        public string L { get; internal set; }
        public string Column1 { get; internal set; }
        public string Production { get; internal set; }
        public string Production100 { get; internal set; }
        public string PercentEff { get; internal set; }
        public string MC2Eff { get; internal set; }
        public string RPMProduction100 { get; internal set; }
        public string Location { get; internal set; }
        public string MachineType { get; internal set; }
        public string MachineNameType { get; internal set; }
        public string Block { get; internal set; }
        public string BlockName { get; internal set; }
        public string MTCLock { get; internal set; }
        public string MTC { get; internal set; }
        public string MTCName { get; internal set; }
        public string MCNo2 { get; internal set; }
        public string MCRPM { get; internal set; }
        public string Row { get; internal set; }
        public string Operator { get; internal set; }
        public string SPYear { get; internal set; }
    }
}
