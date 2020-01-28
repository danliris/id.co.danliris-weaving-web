using System;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class PreparationDailyOperationWarpingCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "PreparationOrder")]
        public OrderId PreparationOrder { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; set; }

        [JsonProperty(PropertyName = "BeamProductResult")]
        public int BeamProductResult { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }

        [JsonProperty(PropertyName = "PreparationShift")]
        public ShiftId PreparationShift { get; set; }

        [JsonProperty(PropertyName = "PreparationOperator")]
        public OperatorId PreparationOperator { get; set; }
    }

    public class PreparationDailyOperationWarpingCommandValidator 
        : AbstractValidator<PreparationDailyOperationWarpingCommand>
    {
        public PreparationDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.PreparationOrder).NotEmpty().WithMessage("No. Order Produksi Harus Diisi");
            RuleFor(command => command.AmountOfCones).NotEmpty().WithMessage("Jumlah Cone Harus Diisi");
            RuleFor(command => command.BeamProductResult).NotEmpty().WithMessage("Jumlah Beam Dihasilkan Harus Diisi");
            RuleFor(command => command.PreparationDate).NotEmpty().WithMessage("Tanggal Pasang Harus Diisi");
            RuleFor(command => command.PreparationTime).NotEmpty().WithMessage("Waktu Pasang Harus Diisi");
            RuleFor(command => command.PreparationShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.PreparationOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
