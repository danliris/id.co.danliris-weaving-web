using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Beams.Commands
{
    public class AddBeamCommand : ICommand<BeamDocument>
    {
        [JsonProperty(propertyName: "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(propertyName: "BeamType")]
        public string BeamType { get; set; }
    }

    public class AddBeamCommandValidator : AbstractValidator<AddBeamCommand>
    {
        public AddBeamCommandValidator()
        {
            RuleFor(command => command.BeamNumber).NotEmpty();
            RuleFor(command => command.BeamType).NotEmpty();
        }
    }

}
