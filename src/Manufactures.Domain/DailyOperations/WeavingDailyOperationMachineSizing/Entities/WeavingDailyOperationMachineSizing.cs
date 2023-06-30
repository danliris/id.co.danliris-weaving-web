using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Productions.ReadModels
{
    public class WeavingDailyOperationMachineSizings : AggregateRoot<WeavingDailyOperationMachineSizings, WeavingDailyOperationMachineSizingReadModel>
    {
        private int v1;
        private string v2;
        private string v3;
        private string v4;
        private string v5;
        private string v6;
        private string v7;
        private string v8;
        private string v9;
        private string v10;
        private string v11;
        private string v12;
        private string v13;
        private string v14;
        private string v15;
        private string v16;
        private string v17;
        private string v18;
        private string v19;
        private string v20;
        private string v21;
        private string v22;
        private string v23;
        private string v24;
        private string v25;
        private string v26;
        private string v27;

        public WeavingDailyOperationMachineSizings(Guid identity) : base(identity)
        {
        }
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
        public DateTimeOffset DeletedDate { get; internal set; }
        public DateTimeOffset ModifiedDate { get; internal set; }

        public WeavingDailyOperationMachineSizings(
            DateTimeOffset ModifiedDate,
            DateTimeOffset DeletedDate,
            Guid identity,
        int PeriodeId ,
         string Periode ,
         string Year ,
         int Date ,
         string Week ,
         string MachineSizing ,
         string Shift ,
         string Group ,
         string Lot ,
         string SP ,
         string YearProduction ,
         string SPYear ,
         string WarpType ,
         string AL ,
         string Construction ,
         string Code ,
         string ThreadOrigin ,
         string Recipe ,
         string Water ,
         string BeamNo ,
         string BeamWidth ,
         string TekSQ ,
         string ThreadCount ,
         string Ne ,
         string TempSD1 ,
         string TempSD2 ,
         string VisCoseSD1 ,
         string VisCoseSD2 ,
         string F1 ,
         string F2,
         string FDS,
         string FW,
         string FP,
         string A12,
         string A34,
         string B12,
         string B34,
         string C1234,
         string Pis,
         string AddedLength,
         string Length,
         string EmptyBeamWeight,
         string Bruto,
         string Netto,
         string Teoritis,
         string SPU,
         string WarpingLenght,
         string FinalCounter,
         string Draft,
         string Speed,
         string Information,
         string SpeedMin,
         string Capacity,
         string Efficiency ) : base(identity)
        {
            
            this.PeriodeId = PeriodeId;
            this.Periode = Periode;
            this.Year = Year;
            this.Date = Date;
            this.Week = Week;
            this.MachineSizing = MachineSizing;
            this.Shift = Shift;
            this.Group = Group;
            this.Lot = Lot;
            this.SP = SP;
            this.YearProduction = YearProduction;
            this.SPYear = SPYear;
            this.WarpType = WarpType;
            this.AL = AL;
            this.Construction = Construction;
            this.Code = Code;
            this.ThreadOrigin = ThreadOrigin;
            this.Recipe = Recipe;
            this.Water = Water;
            this.BeamNo = BeamNo;
            this.BeamWidth = BeamWidth;
            this.TekSQ = TekSQ;
            this.ThreadCount = ThreadCount;
            this.Ne = Ne;
            this.TempSD1 = TempSD1;
            this.TempSD2 = TempSD2;
            this.VisCoseSD1 = VisCoseSD1;
            this.VisCoseSD2 = VisCoseSD2;
            this.F1 = F1;
            this.F2 = F2;
            this.FDS = FDS;
            this.FW = FW;
            this.FP = FP;
            this.A12 = A12;
            this.A34 = A34;
            this.B12 = B12;
            this.B34 = B34;
            this.C1234 = C1234;
            this.Pis = Pis;
            this.AddedLength = AddedLength;
            this.Length = Length;
            this.EmptyBeamWeight = EmptyBeamWeight;
            this.Bruto = Bruto;
            this.Netto = Netto;
            this.Teoritis = Teoritis;
            this.SPU = SPU;
            this.WarpingLenght = WarpingLenght;
            this.FinalCounter = FinalCounter;
            this.Draft = Draft;
            this.Speed = Speed;
            this.Information = Information;
            this.SpeedMin = SpeedMin;
            this.Capacity = Capacity;
            this.Efficiency = Efficiency;
            this.DeletedDate = DeletedDate;
            this.ModifiedDate = ModifiedDate;
            MarkTransient();

           

            ReadModel = new WeavingDailyOperationMachineSizingReadModel(Identity)
            {
                ModifiedDate = this.ModifiedDate,
                DeletedDate = this.DeletedDate,   
            PeriodeId = this.PeriodeId,
            Periode = this.Periode,
            Year = this.Year,
            Date = this.Date,
            Week = this.Week,
            MachineSizing = this.MachineSizing,
            Shift = this.Shift,
            Group = this.Group,
            Lot = this.Lot,
            SP = this.SP,
            YearProduction = this.YearProduction,
            SPYear = this.SPYear,
            WarpType = this.WarpType,
            AL = this.AL,
            Construction = this.Construction,
            Code = this.Code,
            ThreadOrigin = this.ThreadOrigin,
            Recipe = this.Recipe,
            Water = this.Water,
            BeamNo = this.BeamNo,
            BeamWidth = this.BeamWidth,
            TekSQ = this.TekSQ,
            ThreadCount = this.ThreadCount,
            Ne = this.Ne,
            TempSD1 = this.TempSD1,
            TempSD2 = this.TempSD2,
            VisCoseSD1 = this.VisCoseSD1,
            VisCoseSD2 = this.VisCoseSD2,
            F1 = this.F1,
            F2 = this.F2,
            FDS = this.FDS,
            FW = this.FW,
            FP = this.FP,
            A12 = this.A12,
            A34 = this.A34,
            B12 = this.B12,
            B34 = this.B34,
            C1234 = this.C1234,
            Pis = this.Pis,
            AddedLength = this.AddedLength,
            Length = this.Length,
            EmptyBeamWeight = this.EmptyBeamWeight,
            Bruto = this.Bruto,
            Netto = this.Netto,
            Teoritis = this.Teoritis,
            SPU = this.SPU,
            WarpingLenght = this.WarpingLenght,
            FinalCounter = this.FinalCounter,
            Draft = this.Draft,
            Speed = this.Speed,
            Information = this.Information,
            SpeedMin = this.SpeedMin,
            Capacity = this.Capacity,
            Efficiency = this.Efficiency

           
            };
        }

        public WeavingDailyOperationMachineSizings(WeavingDailyOperationMachineSizingReadModel readModel) : base(readModel)
        {

            this.PeriodeId = readModel.PeriodeId;
            this.Periode = readModel.Periode;
            this.Year = readModel.Year;
            this.Date = readModel.Date;
            this.Week = readModel.Week;
            this.MachineSizing = readModel.MachineSizing;
            this.Shift = readModel.Shift;
            this.Group = readModel.Group;
            this.Lot = readModel.Lot;
            this.SP = readModel.SP;
            this.YearProduction = readModel.YearProduction;
            this.SPYear = readModel.SPYear;
            this.WarpType = readModel.WarpType;
            this.AL = readModel.AL;
            this.Construction = readModel.Construction;
            this.Code = readModel.Code;
            this.ThreadOrigin = readModel.ThreadOrigin;
            this.Recipe = readModel.Recipe;
            this.Water = readModel.Water;
            this.BeamNo = readModel.BeamNo;
            this.BeamWidth = readModel.BeamWidth;
            this.TekSQ = readModel.TekSQ;
            this.ThreadCount = readModel.ThreadCount;
            this.Ne = readModel.Ne;
            this.TempSD1 = readModel.TempSD1;
            this.TempSD2 = readModel.TempSD2;
            this.VisCoseSD1 = readModel.VisCoseSD1;
            this.VisCoseSD2 = readModel.VisCoseSD2;
            this.F1 = readModel.F1;
            this.F2 = readModel.F2;
            this.FDS = readModel.FDS;
            this.FW = readModel.FW;
            this.FP = readModel.FP;
            this.A12 = readModel.A12;
            this.A34 = readModel.A34;
            this.B12 = readModel.B12;
            this.B34 = readModel.B34;
            this.C1234 = readModel.C1234;
            this.Pis = readModel.Pis;
            this.AddedLength = readModel.AddedLength;
            this.Length = readModel.Length;
            this.EmptyBeamWeight = readModel.EmptyBeamWeight;
            this.Bruto = readModel.Bruto;
            this.Netto = readModel.Netto;
            this.Teoritis = readModel.Teoritis;
            this.SPU = readModel.SPU;
            this.WarpingLenght = readModel.WarpingLenght;
            this.FinalCounter = readModel.FinalCounter;
            this.Draft = readModel.Draft;
            this.Speed = readModel.Speed;
            this.Information = readModel.Information;
            this.SpeedMin = readModel.SpeedMin;
            this.Capacity = readModel.Capacity;
            this.Efficiency = readModel.Efficiency;
            this.DeletedDate = readModel.DeletedDate.Value;
            this.ModifiedDate = readModel.ModifiedDate.Value;



        }

         protected override WeavingDailyOperationMachineSizings GetEntity()
        {
            return this;
        }
    }
}