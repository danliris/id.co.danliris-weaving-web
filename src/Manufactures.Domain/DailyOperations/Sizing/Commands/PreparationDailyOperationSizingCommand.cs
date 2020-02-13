using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class PreparationDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; set; }

        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; set; }

        [JsonProperty(PropertyName = "NeReal")]
        public double NeReal { get; set; }

        [JsonProperty(PropertyName = "PreparationOperator")]
        public OperatorId PreparationOperator { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }

        [JsonProperty(PropertyName = "PreparationShift")]
        public ShiftId PreparationShift { get; set; }

        [JsonProperty(PropertyName = "BeamProductResult")]
        public int BeamProductResult { get; set; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; set; }

        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; set; }

        [JsonProperty(PropertyName = "BeamsWarping")]
        public List<PreparationDailyOperationSizingBeamsWarpingCommand> BeamsWarping { get; set; }
    }

    public class PreparationDailyOperationSizingCommandValidator : AbstractValidator<PreparationDailyOperationSizingCommand>
    {
        public PreparationDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.MachineDocumentId).NotEmpty().WithMessage("No. Mesin Harus Diisi");
            RuleFor(validator => validator.OrderDocumentId).NotEmpty().WithMessage("No. Order Produksi Harus Diisi");
            RuleFor(validator => validator.RecipeCode).NotEmpty().WithMessage("Kode Resep Harus Diisi");
            RuleFor(validator => validator.NeReal).NotEmpty().WithMessage("Ne Real Harus Diisi");
            RuleFor(validator => validator.PreparationOperator).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.PreparationDate).NotEmpty().WithMessage("Tanggal Pasang Harus Diisi");
            RuleFor(validator => validator.PreparationTime).NotEmpty().WithMessage("Waktu Pasang Harus Diisi");
            RuleFor(validator => validator.PreparationShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(validator => validator.BeamProductResult).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(validator => validator.YarnStrands).NotEmpty().WithMessage("Helai Benang Beam Warping Tidak Boleh 0");
            RuleFor(validator => validator.EmptyWeight).NotEmpty().WithMessage("Berat Kosong Beam Warping Tidak Boleh 0");
            RuleFor(validator => validator.BeamsWarping).NotEmpty().WithMessage("Beam Warping Tidak Boleh Kosong");
        }
    }
}
