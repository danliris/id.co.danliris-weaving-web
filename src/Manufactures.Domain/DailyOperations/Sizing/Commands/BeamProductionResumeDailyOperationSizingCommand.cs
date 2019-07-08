using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class BeamProductionResumeDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {

        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "CounterFinish")]
        public string CounterFinish { get; set; }

        [JsonProperty(PropertyName = "WeightNetto")]
        public string WeightNetto { get; set; }

        [JsonProperty(PropertyName = "WeightBruto")]
        public string WeightBruto { get; set; }

        [JsonProperty(PropertyName = "WeightTheoretical")]
        public string WeightTheoretical { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }
    }

    public class BeamProductionResumeDailyOperationSizingCommandValidator : AbstractValidator<BeamProductionResumeDailyOperationSizingCommand>
    {
        public BeamProductionResumeDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.SizingBeamId.Value).NotEmpty();
            RuleFor(validator => validator.CounterFinish).NotEmpty();
            RuleFor(validator => validator.WeightNetto).NotEmpty();
            RuleFor(validator => validator.WeightBruto).NotEmpty();
            RuleFor(validator => validator.WeightTheoretical).NotEmpty();
            RuleFor(validator => validator.SPU).NotEmpty();
        }
    }
}
