using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class AddNewDailyOperationSizingDetailProductionTimeCommand
    {
        [JsonProperty(PropertyName = "Start")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty(PropertyName = "Pause")]
        public DateTimeOffset Pause { get; set; }

        [JsonProperty(PropertyName = "Resume")]
        public DateTimeOffset Resume { get; set; }

        [JsonProperty(PropertyName = "DoffFinish")]
        public DateTimeOffset Doff { get; set; }
    }

    public class AddNewDailyOperationSizingDetailProductionTimeCommandValidator : AbstractValidator<AddNewDailyOperationSizingDetailProductionTimeCommand>
    {
        public AddNewDailyOperationSizingDetailProductionTimeCommandValidator()
        {
            RuleFor(validator => validator.Start).NotEmpty();
        }
    }
}
