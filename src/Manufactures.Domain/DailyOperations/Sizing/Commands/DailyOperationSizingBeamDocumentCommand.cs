using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingBeamDocumentCommand
    {
        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingCounterCommand Counter { get; set; }

        //[JsonProperty(PropertyName = "Weight")]
        //public DailyOperationSizingWeightCommand Weight { get; set; }

        //[JsonProperty(PropertyName = "PISMeter")]
        //public double PISMeter { get; set; }

        //[JsonProperty(PropertyName = "SPU")]
        //public double SPU { get; set; }

        //[JsonProperty(PropertyName = "SizingBeamStatus")]
        //public string SizingBeamStatus { get; set; }
    }

    public class DailyOperationSizingBeamDocumentCommandValidator : AbstractValidator<DailyOperationSizingBeamDocumentCommand>
    {
        public DailyOperationSizingBeamDocumentCommandValidator()
        {
            RuleFor(validator => validator.SizingBeamId).NotEmpty();
            RuleFor(validator => validator.Counter).SetValidator(new DailyOperationSizingCounterCommandValidator());
            //RuleFor(validator => validator.Weight).SetValidator(new DailyOperationSizingWeightCommandValidator());
        }
    }
}
