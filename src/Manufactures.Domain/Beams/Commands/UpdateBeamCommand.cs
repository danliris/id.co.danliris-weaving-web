using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Beams.Commands
{
    public class UpdateBeamCommand : ICommand<BeamDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(propertyName: "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(propertyName: "BeamType")]
        public string BeamType { get; set; }
    }

    public class UpdateBeamCommandValidator : AbstractValidator<UpdateBeamCommand>
    {
        public UpdateBeamCommandValidator()
        {
            RuleFor(command => command.Id).NotNull();
            RuleFor(command => command.BeamNumber).NotEmpty();
            RuleFor(command => command.BeamType).NotEmpty();
        }
    }
}
