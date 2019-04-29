using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Beams.Commands
{
    public class AddBeamCommand : ICommand<BeamDocument>
    {
        [JsonProperty(propertyName: "BeamCode")]
        public string BeamCode { get; set; }

        [JsonProperty(propertyName: "BeamType")]
        public string BeamType { get; set; }
    }

    public class AddBeamCommandValidator : AbstractValidator<AddBeamCommand>
    {
        public AddBeamCommandValidator()
        {
            RuleFor(command => command.BeamCode).NotEmpty();
            RuleFor(command => command.BeamType).NotEmpty();
        }
    }

}
