using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class SizingCauseCommand
    {
        [JsonProperty(PropertyName = "BrokenBeam")]
        public string BrokenBeam { get; set; }

        [JsonProperty(PropertyName = "MachineTroubled")]
        public string MachineTroubled { get; set; }
    }
    public class SizingCausesCommandValidator
      : AbstractValidator<SizingCauseCommand>
    {
        public SizingCausesCommandValidator()
        {
            RuleFor(validator => validator.BrokenBeam).NotEmpty().Unless(command => string.IsNullOrEmpty(command.MachineTroubled));
            RuleFor(validator => validator.MachineTroubled).NotEmpty().Unless(command => string.IsNullOrEmpty(command.BrokenBeam));
        }
    }
}
