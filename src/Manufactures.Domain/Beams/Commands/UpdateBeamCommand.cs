using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Beams.Commands
{
    public class UpdateBeamCommand : ICommand<BeamDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(propertyName: "Number")]
        public string Number { get; set; }

        [JsonProperty(propertyName: "Type")]
        public string Type { get; set; }

        [JsonProperty(propertyName: "EmtpyWeight")]
        public double EmtpyWeight { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateBeamCommandValidator : AbstractValidator<UpdateBeamCommand>
    {
        public UpdateBeamCommandValidator()
        {
            RuleFor(command => command.Id).NotNull();
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.Type).NotEmpty();
        }
    }
}
