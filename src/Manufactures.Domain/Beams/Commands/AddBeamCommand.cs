using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Beams.Commands
{
    public class AddBeamCommand : ICommand<BeamDocument>
    {
        [JsonProperty(propertyName: "Number")]
        public string Number { get; set; }

        [JsonProperty(propertyName: "Type")]
        public string Type { get; set; }

        [JsonProperty(propertyName: "EmptyWeight")]
        public double EmptyWeight { get; set; }
    }

    public class AddBeamCommandValidator : AbstractValidator<AddBeamCommand>
    {
        public AddBeamCommandValidator()
        {
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.Type).NotEmpty();
            RuleFor(command => command.EmptyWeight).NotEmpty();
        }
    }

}
