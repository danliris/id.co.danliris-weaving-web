using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class ProduceBeamBeamDocumentDailyOperationSizingCommand
    {
        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingCounterCommand Counter { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        public DailyOperationSizingWeightCommand Weight { get; set; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }
    }
    public class ProduceBeamBeamDocumentDailyOperationSizingCommandValidator : AbstractValidator<ProduceBeamBeamDocumentDailyOperationSizingCommand>
    {
        public ProduceBeamBeamDocumentDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.SizingBeamId).NotEmpty();
            RuleFor(validator => validator.Counter).SetValidator(new DailyOperationSizingCounterCommandValidator());
            RuleFor(validator => validator.Weight).SetValidator(new DailyOperationSizingWeightCommandValidator());
            RuleFor(validator => validator.PISMeter).NotEmpty();
            RuleFor(validator => validator.SPU).NotEmpty();
        }
    }
}
