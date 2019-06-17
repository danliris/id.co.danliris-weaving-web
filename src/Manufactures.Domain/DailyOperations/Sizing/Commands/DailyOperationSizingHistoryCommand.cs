using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingHistoryCommand
    {
        [JsonProperty(PropertyName = "MachineDate")]
        public DateTimeOffset MachineDate { get; set; }

        [JsonProperty(PropertyName = "MachineTime")]
        public TimeSpan MachineTime { get; set; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }
    }

    public class DailyOperationSizingHistoryCommandValidator
       : AbstractValidator<DailyOperationSizingHistoryCommand>
    {
        public DailyOperationSizingHistoryCommandValidator()
        {
            RuleFor(command => command.MachineDate).NotNull();
            RuleFor(command => command.MachineTime).NotNull();
        }
    }
}
