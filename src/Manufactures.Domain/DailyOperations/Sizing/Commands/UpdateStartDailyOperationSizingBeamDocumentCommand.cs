using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateStartDailyOperationSizingBeamDocumentCommand
    {
        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }
    }

    public class UpdateStartDailyOperationSizingBeamDocumentCommandValidator : AbstractValidator<UpdateStartDailyOperationSizingBeamDocumentCommand>
    {
        public UpdateStartDailyOperationSizingBeamDocumentCommandValidator()
        {
            RuleFor(validator => validator.SizingBeamId).NotEmpty();
        }
    }
}
