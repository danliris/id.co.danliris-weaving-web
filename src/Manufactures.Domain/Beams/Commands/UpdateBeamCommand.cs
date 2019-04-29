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

        [JsonProperty(propertyName: "BeamCode")]
        public string BeamCode { get; set; }

        [JsonProperty(propertyName: "BeamType")]
        public string BeamType { get; set; }
    }

    public class UpdateBeamCommandValidator : AbstractValidator<UpdateBeamCommand>
    {
        public UpdateBeamCommandValidator()
        {
            RuleFor(command => command.Id).NotNull();
            RuleFor(command => command.BeamCode).NotEmpty();
            RuleFor(command => command.BeamType).NotEmpty();
        }
    }
}
