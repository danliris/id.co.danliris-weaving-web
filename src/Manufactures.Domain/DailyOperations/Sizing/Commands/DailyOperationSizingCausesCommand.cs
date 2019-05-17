using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingCausesCommand
    {
        [JsonProperty(PropertyName = "BrokenBeam")]
        public string BrokenBeam { get; set; }

        [JsonProperty(PropertyName = "MachineTroubled")]
        public string MachineTroubled { get; set; }
    }
    public class DailyOperationSizingCausesCommandValidator
      : AbstractValidator<DailyOperationSizingCausesCommand>
    {
        public DailyOperationSizingCausesCommandValidator()
        {
            RuleFor(command => command.BrokenBeam).NotEmpty().Unless(command => string.IsNullOrEmpty(command.MachineTroubled));
            RuleFor(command => command.MachineTroubled).NotEmpty().Unless(command => string.IsNullOrEmpty(command.BrokenBeam));
        }
    }
}
