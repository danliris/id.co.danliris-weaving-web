using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingCauseCommand
    {
        [JsonProperty(PropertyName = "BrokenBeam")]
        public string BrokenBeam { get; set; }

        [JsonProperty(PropertyName = "MachineTroubled")]
        public string MachineTroubled { get; set; }

        public DailyOperationSizingCauseCommand(string brokenBeam, string machineTroubled)
        {
            BrokenBeam = brokenBeam;
            MachineTroubled = machineTroubled;
        }
    }
    public class DailyOperationSizingCausesCommandValidator
      : AbstractValidator<DailyOperationSizingCauseCommand>
    {
        public DailyOperationSizingCausesCommandValidator()
        {
            RuleFor(validator => validator.BrokenBeam).NotEmpty().Unless(command => string.IsNullOrEmpty(command.MachineTroubled));
            RuleFor(validator => validator.MachineTroubled).NotEmpty().Unless(command => string.IsNullOrEmpty(command.BrokenBeam));
        }
    }
}
