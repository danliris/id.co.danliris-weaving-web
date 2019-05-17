using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingHistoryCommand
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

    public class DailyOperationSizingHistoryCommandValidator
       : AbstractValidator<DailyOperationSizingHistoryCommand>
    {
        public DailyOperationSizingHistoryCommandValidator()
        {
            RuleFor(command => command.TimeOnMachine).NotNull();
            RuleFor(command => command.MachineStatus).NotEmpty();
        }
    }
}
