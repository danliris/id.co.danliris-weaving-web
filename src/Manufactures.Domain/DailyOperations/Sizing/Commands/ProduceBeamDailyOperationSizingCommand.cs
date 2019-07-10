using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class ProduceBeamDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "CounterFinish")]
        public string CounterFinish { get; set; }

        [JsonProperty(PropertyName = "WeightNetto")]
        public double WeightNetto { get; set; }

        [JsonProperty(PropertyName = "WeightBruto")]
        public double WeightBruto { get; set; }

        [JsonProperty(PropertyName = "WeightTheoretical")]
        public double WeightTheoretical { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }

        [JsonProperty(PropertyName = "SizingBeamStatus")]
        public string SizingBeamStatus { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ProduceBeamDailyOperationSizingCommandValidator : AbstractValidator<ProduceBeamDailyOperationSizingCommand>
    {
        public ProduceBeamDailyOperationSizingCommandValidator()
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
