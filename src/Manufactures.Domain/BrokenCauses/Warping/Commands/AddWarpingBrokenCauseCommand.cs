using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BrokenCauses.Warping.Commands
{
    public class AddWarpingBrokenCauseCommand : ICommand<WarpingBrokenCauseDocument>
    {
        [JsonProperty(propertyName: "WarpingBrokenCauseName")]
        public string WarpingBrokenCauseName { get; set; }

        [JsonProperty(propertyName: "Information")]
        public string Information { get; set; }

        [JsonProperty(propertyName: "WarpingBrokenCauseCategory")]
        public string WarpingBrokenCauseCategory { get; set; }
    }

    public class AddWarpingBrokenCauseCommandValidator : AbstractValidator<AddWarpingBrokenCauseCommand>
    {
        public AddWarpingBrokenCauseCommandValidator()
        {
            RuleFor(command => command.WarpingBrokenCauseName).NotEmpty();
            RuleFor(command => command.WarpingBrokenCauseCategory).NotEmpty();
        }
    }
}
