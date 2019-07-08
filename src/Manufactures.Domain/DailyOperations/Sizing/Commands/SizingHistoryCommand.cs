using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class SizingHistoryCommand
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

    public class SizingHistoryCommandValidator
       : AbstractValidator<SizingHistoryCommand>
    {
        public SizingHistoryCommandValidator()
        {
            RuleFor(validator => validator.MachineDate).NotNull();
            RuleFor(validator => validator.MachineTime).NotNull();
        }
    }
}
