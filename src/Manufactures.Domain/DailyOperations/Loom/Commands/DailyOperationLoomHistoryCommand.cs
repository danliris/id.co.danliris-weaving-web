using FluentValidation;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class DailyOperationLoomHistoryCommand
    {
        [JsonProperty(PropertyName = "TimeOnMachine")]
        public DateTimeOffset TimeOnMachine { get; set; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; private set; }

        [JsonProperty(PropertyName = "IsUp")]
        public bool IsUp { get; private set; }

        [JsonProperty(PropertyName = "IsDown")]
        public bool IsDown { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }
    }

    public class DailyOperationLoomHistoryCommandValidator 
        : AbstractValidator<DailyOperationLoomHistoryCommand>
    {
        public DailyOperationLoomHistoryCommandValidator()
        {
            RuleFor(command => command.TimeOnMachine).NotNull();
            RuleFor(command => command.MachineStatus).NotEmpty();
        }
    }
}
