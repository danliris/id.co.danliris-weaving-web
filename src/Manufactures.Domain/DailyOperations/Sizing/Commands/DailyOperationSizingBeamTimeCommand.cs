using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingBeamTimeCommand
    {
        [JsonProperty(PropertyName = "UpTime")]
        public DateTimeOffset UpTime { get; set; }

        [JsonProperty(PropertyName = "DownTime")]
        public DateTimeOffset DownTime { get; set; }
    }
    public class DailyOperationSizingBeamTimeCommandValidator
        : AbstractValidator<DailyOperationSizingBeamTimeCommand>
    {
        public DailyOperationSizingBeamTimeCommandValidator()
        {
            RuleFor(validator => validator.UpTime).NotEmpty();
            RuleFor(validator => validator.DownTime).NotEmpty();
        }
    }
}
