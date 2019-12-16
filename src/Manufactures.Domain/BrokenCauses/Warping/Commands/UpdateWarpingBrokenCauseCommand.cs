using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BrokenCauses.Warping.Commands
{
    public class UpdateWarpingBrokenCauseCommand : ICommand<WarpingBrokenCauseDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(propertyName: "WarpingBrokenCauseName")]
        public string WarpingBrokenCauseName { get; set; }

        [JsonProperty(propertyName: "Information")]
        public string Information { get; set; }

        [JsonProperty(propertyName: "WarpingBrokenCauseCategory")]
        public string WarpingBrokenCauseCategory { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateWarpingBrokenCauseCommandValidator : AbstractValidator<UpdateWarpingBrokenCauseCommand>
    {
        public UpdateWarpingBrokenCauseCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.WarpingBrokenCauseName).NotEmpty();
            RuleFor(command => command.WarpingBrokenCauseCategory).NotEmpty();
        }
    }
}
