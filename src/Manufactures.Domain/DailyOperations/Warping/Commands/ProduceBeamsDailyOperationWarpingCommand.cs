using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class ProduceBeamsDailyOperationWarpingCommand
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

        //[JsonProperty(PropertyName = "WarpingBeamLengthUomId")]
        //public int WarpingBeamLengthUomId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ProduceBeamsDailyOperationWarpingCommandValidator : AbstractValidator<ProduceBeamsDailyOperationWarpingCommand>
    {
        public ProduceBeamsDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.ProduceBeamsDate).NotEmpty().WithMessage("Tanggal Produksi Beam Harus Diisi");
            RuleFor(command => command.ProduceBeamsTime).NotEmpty().WithMessage("Waktu Produksi Beam Harus Diisi");
            RuleFor(command => command.ProduceBeamsShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.ProduceBeamsOperator).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(command => command.WarpingBeamLengthPerOperator).NotEmpty().WithMessage("Panjang Beam Warping Harus Diisi");
            //RuleFor(command => command.WarpingBeamLengthUomId).NotEmpty().WithMessage("Satuan Panjang Beam Warping Harus Diisi");
        }
    }
}
