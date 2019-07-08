using FluentValidation;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class SizingWeightCommand
    {
        [JsonProperty(PropertyName = "Netto")]
        public double Netto { get; set; }

        [JsonProperty(PropertyName = "Bruto")]
        public double Bruto { get; set; }

        [JsonProperty(PropertyName = "Theoretical")]
        public double Theoretical { get; set; }
    }

    public class SizingWeightCommandValidator
      : AbstractValidator<SizingWeightCommand>
    {
        public SizingWeightCommandValidator()
        {
            RuleFor(validator => validator.Netto).NotEmpty();
            RuleFor(validator => validator.Bruto).NotEmpty();
            RuleFor(validator => validator.Theoretical).NotEmpty();
        }
    }
}
