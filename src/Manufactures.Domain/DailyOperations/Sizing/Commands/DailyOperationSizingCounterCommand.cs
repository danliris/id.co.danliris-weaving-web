using FluentValidation;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingCounterCommand
    {
        [JsonProperty(PropertyName = "Start")]
        public double Start { get; set; }

        [JsonProperty(PropertyName = "Finish")]
        public double Finish { get; set; }

        //public DailyOperationSizingCounterCommand(DailyOperationSizingCounterCommand counter)
        //{
        //    Start = counter.Start;
        //    Finish = counter.Finish;
        //}
    }

    public class DailyOperationSizingCounterCommandValidator
      : AbstractValidator<DailyOperationSizingCounterCommand>
    {
        public DailyOperationSizingCounterCommandValidator()
        {
            RuleFor(validator => validator.Start >= 0);
            RuleFor(validator => validator.Finish).NotEmpty().Unless(validator => validator.Start >= 0);
        }
    }
}
