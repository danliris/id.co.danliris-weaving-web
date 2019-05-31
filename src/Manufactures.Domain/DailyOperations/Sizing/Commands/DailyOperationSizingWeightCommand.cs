using FluentValidation;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingWeightCommand
    {
        [JsonProperty(PropertyName = "Netto")]
        public string Netto { get; set; }

        [JsonProperty(PropertyName = "Bruto")]
        public string Bruto { get; set; }
    }

    //public class DailyOperationSizingWeightCommandValidator
    //  : AbstractValidator<DailyOperationSizingWeightCommand>
    //{
    //    public DailyOperationSizingWeightCommandValidator()
    //    {
    //        RuleFor(command => command.Netto).NotEmpty();
    //        RuleFor(command => command.Bruto).NotEmpty().Unless(command => !string.IsNullOrEmpty(command.Netto));
    //    }
    //}
}
