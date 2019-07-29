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

        [JsonProperty(PropertyName = "SizingBeamDocuments")]
        public ProduceBeamBeamDocumentDailyOperationSizingCommand SizingBeamDocuments { get; set; }

        [JsonProperty(PropertyName = "SizingDetails")]
        public ProduceBeamDetailDailyOperationSizingCommand SizingDetails { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ProduceBeamDailyOperationSizingCommandValidator : AbstractValidator<ProduceBeamDailyOperationSizingCommand>
    {
        public ProduceBeamDailyOperationSizingCommandValidator()
        {
            //RuleFor(validator => validator.SizingBeamId.Value).NotEmpty();
            //RuleFor(validator => validator.CounterFinish).NotEmpty();
            //RuleFor(validator => validator.WeightNetto).NotEmpty();
            //RuleFor(validator => validator.WeightBruto).NotEmpty();
            //RuleFor(validator => validator.WeightTheoretical).NotEmpty();
            //RuleFor(validator => validator.SPU).NotEmpty();
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.SizingBeamDocuments).SetValidator(new ProduceBeamBeamDocumentDailyOperationSizingCommandValidator());
            RuleFor(validator => validator.SizingDetails).SetValidator(new ProduceBeamDetailDailyOperationSizingCommandValidator());
        }
    }
}
