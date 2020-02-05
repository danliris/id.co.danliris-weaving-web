using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class PreparationDailyOperationSizingBeamsWarpingCommand
    {
        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }
        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; set; }
        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; set; }
    }

    public class PreparationDailyOperationSizingBeamsWarpingCommandValidator : AbstractValidator<PreparationDailyOperationSizingBeamsWarpingCommand>
    {
        public PreparationDailyOperationSizingBeamsWarpingCommandValidator()
        {
            RuleFor(validator => validator.BeamDocumentId).NotEmpty().WithMessage("Beam Harus Diisi");
            RuleFor(validator => validator.YarnStrands).NotEmpty().WithMessage("Helai Benang Tidak Boleh Kosong");
            RuleFor(validator => validator.EmptyWeight).NotEmpty().WithMessage("Berat Kosong Tidak Boleh Kosong");
        }
    }
}
