using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Rings.Commands
{
    public class CreateRingDocumentCommand : ICommand<RingDocument>
    {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "RingType")]
        public string RingType { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
    }

    public class CreateRingDocumentCommandValidator : AbstractValidator<CreateRingDocumentCommand>
    {
        public CreateRingDocumentCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.RingType).NotEmpty();
        }
    }
}
