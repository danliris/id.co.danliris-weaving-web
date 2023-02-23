using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class UpdateStartDailyOperationWarpingCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty(PropertyName = "StartTime")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty(PropertyName = "StartShift")]
        public ShiftId StartShift { get; set; }

        [JsonProperty(PropertyName = "StartOperator")]
        public OperatorId StartOperator { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamId")]
        public BeamId WarpingBeamId { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamLengthUomId")]
        public UomId WarpingBeamLengthUomId { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamLengthUomUnit")]
        public string WarpingBeamLengthUomUnit { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateStartDailyOperationWarpingCommandValidator : AbstractValidator<UpdateStartDailyOperationWarpingCommand>
    {
        public UpdateStartDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.StartDate).NotEmpty().WithMessage("Tanggal Mulai Harus Diisi");
            RuleFor(command => command.StartTime).NotEmpty().WithMessage("Waktu Mulai Harus Diisi");
            RuleFor(command => command.StartShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.StartOperator).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(command => command.WarpingBeamId).NotEmpty().WithMessage("No. Beam Warping Harus Diisi");
            RuleFor(command => command.WarpingBeamLengthUomId).NotEmpty().WithMessage("Satuan Panjang Beam Warping Harus Diisi");
            RuleFor(command => command.WarpingBeamLengthUomUnit).NotEmpty().WithMessage("Satuan Panjang Beam Warping Harus Diisi");
        }
    }
}
