using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomMachine : AggregateRoot<DailyOperationLoomMachine, DailyOperationLoomMachineReadModel>
    {
        protected override DailyOperationLoomMachine GetEntity()
        {
            return this;
        }
        public DailyOperationLoomMachine(Guid identity) : base(identity)
        {
        }

        public DailyOperationLoomMachine(Guid identity,int date, string monthPeriode, int monthPeriodeId, string yearPeriode, 
            string shift, string mCNo, string sPNo, string year, string tA, string warp, string weft, string length, string warpType, 
            string weftType, string weftType2, string weftType3, string aL, string aP, string aP2, string aP3, string thread, 
            string construction, string threadType, string monthId, string productionCMPX, string eFFMC, string rPM, string t, string f, 
            string w, string l, string column1, string production, string production100, string percentEff, string mC2Eff, 
            string rPMProduction100, string location, string machineType, string machineNameType, string block, string blockName, 
            string mTCLock, string mTC, string mtcName, string mCNo2, string mCRPM, string row, string @operator, string sPYear) : base(identity)
        {
            Date = date;
            MonthPeriode = monthPeriode;
            MonthPeriodeId = monthPeriodeId;
            YearPeriode = yearPeriode;
            Shift = shift;
            MCNo = mCNo;
            SPNo = sPNo;
            Year = year;
            TA = tA;
            Warp = warp;
            Weft = weft;
            Length = length;
            WarpType = warpType;
            WeftType = weftType;
            WeftType2 = weftType2;
            WeftType3 = weftType3;
            AL = aL;
            AP = aP;
            AP2 = aP2;
            AP3 = aP3;
            Thread = thread;
            Construction = construction;
            ThreadType = threadType;
            MonthId = monthId;
            ProductionCMPX = productionCMPX;
            EFFMC = eFFMC;
            RPM = rPM;
            T = t;
            F = f;
            W = w;
            L = l;
            Column1 = column1;
            Production = production;
            Production100 = production100;
            PercentEff = percentEff;
            MC2Eff = mC2Eff;
            RPMProduction100 = rPMProduction100;
            Location = location;
            MachineType = machineType;
            MachineNameType = machineNameType;
            Block = block;
            BlockName = blockName;
            MTCLock = mTCLock;
            MTC = mTC;
            MCNo2 = mCNo2;
            MCRPM = mCRPM;
            Row = row;
            Operator = @operator;
            SPYear = sPYear;
            MTCName = mtcName;
            MarkTransient();

            ReadModel = new DailyOperationLoomMachineReadModel(Identity)
            {
                Date = this.Date,
                MonthPeriode = this.MonthPeriode,
                MonthPeriodeId = this.MonthPeriodeId,
                Year = this.Year,
                YearPeriode = this.YearPeriode,
                Shift = this.Shift,
                MCNo = this.MCNo,
                AL = this.AL,
                AP = this.AP,
                AP2 = this.AP2,
                AP3 = this.AP3,
                TA = this.TA,
                Block = this.Block,
                BlockName = this.BlockName,
                Column1 = this.Column1,
                Construction = this.Construction,
                EFFMC = this.EFFMC,
                MC2Eff = this.MC2Eff,
                MTC = this.MTC,
                MachineNameType = this.MachineNameType,
                MachineType = this.MachineType,
                L = this.L,
                Operator = this.Operator,
                Length = this.Length,
                Location = this.Location,
                F = this.F,
                MCNo2 = this.MCNo2,
                MCRPM = this.MCRPM,
                MonthId = this.MonthId,
                MTCLock = this.MTCLock,
                PercentEff= this.PercentEff,
                Production= this.Production,
                Production100= this.Production100,
                ProductionCMPX= this.ProductionCMPX,
                RPM= this.RPM,
                Row= this.Row,
                RPMProduction100=this.RPMProduction100,
                SPNo=this.SPNo,
                SPYear=this.SPYear,
                T=this.T,
                Thread=this.Thread,
                ThreadType=this.ThreadType,
                WarpType=this.WarpType,
                W= this.W,
                Warp= this.Warp,
                Weft= this.Weft,
                WeftType= this.WeftType,
                WeftType2= this.WeftType2,
                WeftType3= this.WeftType3,
                MTCName=this.MTCName
               
            };
        }

        public DailyOperationLoomMachine(DailyOperationLoomMachineReadModel readModel) : base(readModel)
        {
            Date = readModel.Date;
            MonthPeriode = readModel.MonthPeriode;
            MonthPeriodeId = readModel.MonthPeriodeId;
            Year = readModel.Year;
            YearPeriode = readModel.YearPeriode;
            Shift = readModel.Shift;
            MCNo = readModel.MCNo;
            AL = readModel.AL;
            AP = readModel.AP;
            AP2 = readModel.AP2;
            AP3 = readModel.AP3;
            TA = readModel.TA;
            Block = readModel.Block;
            BlockName = readModel.BlockName;
            Column1 = readModel.Column1;
            Construction = readModel.Construction;
            EFFMC = readModel.EFFMC;
            MC2Eff = readModel.MC2Eff;
            MTC = readModel.MTC;
            MachineNameType = readModel.MachineNameType;
            MachineType = readModel.MachineType;
            L = readModel.L;
            Operator = readModel.Operator;
            Length = readModel.Length;
            Location = readModel.Location;
            F = readModel.F;
            MCNo2 = readModel.MCNo2;
            MCRPM = readModel.MCRPM;
            MonthId = readModel.MonthId;
            MTCLock = readModel.MTCLock;
            PercentEff = readModel.PercentEff;
            Production = readModel.Production;
            Production100 = readModel.Production100;
            ProductionCMPX = readModel.ProductionCMPX;
            RPM = readModel.RPM;
            Row = readModel.Row;
            RPMProduction100 = readModel.RPMProduction100;
            SPNo = readModel.SPNo;
            SPYear = readModel.SPYear;
            T = readModel.T;
            Thread = readModel.Thread;
            ThreadType = readModel.ThreadType;
            WarpType = readModel.WarpType;
            W = readModel.W;
            Warp = readModel.Warp;
            Weft = readModel.Weft;
            WeftType = readModel.WeftType;
            WeftType2 = readModel.WeftType2;
            WeftType3 = readModel.WeftType3;
            MTCName = readModel.MTCName;
        }


        public int Date { get; internal set; }
        public string MonthPeriode { get; internal set; }
        public int MonthPeriodeId { get; internal set; }
        public string YearPeriode { get; internal set; }
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
