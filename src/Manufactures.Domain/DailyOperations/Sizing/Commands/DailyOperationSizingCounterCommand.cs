using FluentValidation;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingCounterCommand
    {
        [JsonProperty(PropertyName = "Start")]
        public string Start { get; set; }

        [JsonProperty(PropertyName = "Finish")]
        public string Finish { get; set; }
    }

    //public class DailyOperationSizingCounterCommandValidator
    //  : AbstractValidator<DailyOperationSizingCounterCommand>
    //{
    //    public DailyOperationSizingCounterCommandValidator()
    //    {
    //        RuleFor(command => command.Start).NotEmpty();
    //        RuleFor(command => command.Finish).NotEmpty().Unless(command => !string.IsNullOrEmpty(command.Start));
    //    }
    //}
}
