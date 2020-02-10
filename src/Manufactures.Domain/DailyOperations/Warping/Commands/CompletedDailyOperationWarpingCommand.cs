using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class CompletedDailyOperationWarpingCommand
         : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsDate")]
        public DateTimeOffset ProduceBeamsDate { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsTime")]
        public TimeSpan ProduceBeamsTime { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsShift")]
        public ShiftId ProduceBeamsShift { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsOperator")]
        public OperatorId ProduceBeamsOperator { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamLengthPerOperator")]
        public double WarpingBeamLengthPerOperator { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamLengthUomId")]
        public int WarpingBeamLengthUomId { get; set; }

        [JsonProperty(PropertyName = "Tention")]
        public int Tention { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "PressRoll")]
        public double PressRoll { get; set; }

        [JsonProperty(PropertyName = "PressRollUom")]
        public string PressRollUom { get; set; }

        [JsonProperty(PropertyName = "BrokenCauses")]
        public List<WarpingBrokenThreadsCausesCommand> BrokenCauses { get; set; }

        [JsonProperty(PropertyName = "IsFinishFlag")]
        public bool IsFinishFlag { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class CompletedDailyOperationWarpingCommandValidator : AbstractValidator<CompletedDailyOperationWarpingCommand>
    {
        public CompletedDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.ProduceBeamsDate).NotEmpty().WithMessage("Tanggal Produksi Beam Harus Diisi");
            RuleFor(command => command.ProduceBeamsTime).NotEmpty().WithMessage("Waktu Produksi Beam Harus Diisi");
            RuleFor(command => command.ProduceBeamsShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.ProduceBeamsOperator).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(command => command.WarpingBeamLengthPerOperator).NotEmpty().WithMessage("Panjang Beam Warping Harus Diisi");
            RuleFor(command => command.WarpingBeamLengthUomId).NotEmpty().WithMessage("Satuan Panjang Beam Warping Harus Diisi");
            RuleFor(command => command.Tention).NotEmpty().WithMessage("Tention Harus Diisi");
            RuleFor(command => command.MachineSpeed).NotEmpty().WithMessage("Machine Speed Harus Diisi");
            RuleFor(command => command.PressRoll).NotEmpty().WithMessage("Press Roll Harus Diisi");
            RuleFor(command => command.PressRollUom).NotEmpty().WithMessage("Satuan Press Roll Harus Diisi");
            //RuleFor(command => command.BrokenCauses).NotEmpty();
            RuleFor(command => command.IsFinishFlag).NotNull();
        }
    }
}
