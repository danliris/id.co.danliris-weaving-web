using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class WeavingDailyOperationWarpingMachineCommand
           : ICommand<WeavingDailyOperationWarpingMachine>
    {
        public Stream stream { get; set; }
        //public DateTime Day { get;  set; }
        //public int Month { get;  set; }
        //public int Year { get;  set; }
        //public string Shift { get;  set; }
        //public string MCNo { get;  set; }
        //public string Name { get;  set; }
        //public string Lot { get;  set; }
        //public string SP { get;  set; }
        //public string YearSP { get;  set; }
        //public string WarpType { get;  set; }
        //public string Code { get;  set; }
        //public string BeamNo { get;  set; }
        //public int TotalCone { get;  set; }
        //public string ThreadNo { get;  set; }
        //public double Length { get;  set; }
        //public string Uom { get;  set; }
        //public DateTime Start { get;  set; }
        //public DateTime Doff { get;  set; }
        //public double HNLeft { get;  set; }
        //public double HNMiddle { get;  set; }
        //public double HNRight { get;  set; }
        //public double SpeedMeterPerMinute { get;  set; }
        //public double ThreadCut { get;  set; }
        //public double Capacity { get;  set; }
        //public double Eff { get;  set; }

    }
    public class WeavingDailyOperationWarpingMachineCommandValidator : AbstractValidator<WeavingDailyOperationWarpingMachineCommand>
    {
        public WeavingDailyOperationWarpingMachineCommandValidator()
        {



            //var ws = excelPackage.Workbook.Worksheets[0];

            //var storage = new string[2] { "", "" };
            //if (!string.IsNullOrWhiteSpace((string)ws.Cells["B3"].Value))
            //{
            //    storage = ((string)ws.Cells["B3"].Value).Split("-");
            //}
            var commands = new WeavingDailyOperationWarpingMachineCommand();
            RuleFor(command => command.stream).NotEmpty();
            //RuleFor(command => command.ProduceBeamsDate).NotEmpty().WithMessage("Tanggal Produksi Beam Harus Diisi");
            //RuleFor(command => command.ProduceBeamsTime).NotEmpty().WithMessage("Waktu Produksi Beam Harus Diisi");
            //RuleFor(command => command.ProduceBeamsShift).NotEmpty().WithMessage("Shift Harus Diisi");
            //RuleFor(command => command.ProduceBeamsOperator).NotEmpty().WithMessage("Operator Harus Diisi");
            //RuleFor(command => command.WarpingBeamLengthPerOperator).NotEmpty().WithMessage("Panjang Beam Warping Harus Diisi");
            ////RuleFor(command => command.WarpingBeamLengthUomId).NotEmpty().WithMessage("Satuan Panjang Beam Warping Harus Diisi");
            //RuleFor(command => command.Tention).NotEmpty().WithMessage("Tention Harus Diisi");
            //RuleFor(command => command.MachineSpeed).NotEmpty().WithMessage("Machine Speed Harus Diisi");
            //RuleFor(command => command.PressRoll).NotEmpty().WithMessage("Press Roll Harus Diisi");
            //RuleFor(command => command.PressRollUom).NotEmpty().WithMessage("Satuan Press Roll Harus Diisi");
            ////RuleFor(command => command.BrokenCauses).NotEmpty();
            //RuleFor(command => command.IsFinishFlag).NotNull();
            //RuleFor(command => command.ProduceBeamsId).NotEmpty().WithMessage("Warping Beams harus Diiisi");
        }
    }
}
