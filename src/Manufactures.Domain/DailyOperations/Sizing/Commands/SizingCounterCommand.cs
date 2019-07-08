using FluentValidation;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class SizingCounterCommand
    {
        [JsonProperty(PropertyName = "Start")]
        public double Start { get; set; }

        [JsonProperty(PropertyName = "Finish")]
        public double Finish { get; set; }
    }

    public class SizingCounterCommandValidator
      : AbstractValidator<SizingCounterCommand>
    {
        public SizingCounterCommandValidator()
        {
            RuleFor(validator => validator.Start).NotEmpty();
            RuleFor(validator => validator.Finish).NotEmpty().Unless(validator => !validator.Start.Equals(0) && (validator.Start > 0));
        }
    }
}
