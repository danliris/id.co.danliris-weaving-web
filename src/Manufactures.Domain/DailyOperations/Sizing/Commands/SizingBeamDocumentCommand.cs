using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class SizingBeamDocumentCommand
    {
        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public SizingCounterCommand Counter { get; set; }
    }

    public class SizingBeamDocumentCommandValidator : AbstractValidator<SizingBeamDocumentCommand>
    {
        public SizingBeamDocumentCommandValidator()
        {
            RuleFor(validator => validator.SizingBeamId).NotEmpty();
            RuleFor(validator => validator.Counter).SetValidator(new SizingCounterCommandValidator());
        }
    }
}
