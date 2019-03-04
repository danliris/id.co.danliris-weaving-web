using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.Yarns.Commands
{
    public class CreateNewYarnCommand : ICommand<YarnDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Tags")]
        public string Tags { get; set; }

        [JsonProperty(PropertyName = "MaterialTypeDocument")]
        public MaterialTypeValueObject MaterialTypeDocument { get; set; }

        [JsonProperty(PropertyName = "RingDocument")]
        public YarnNumberValueObject RingDocument { get; set; }
    }

    public class CreateNewYarnCommandValidator : AbstractValidator<CreateNewYarnCommand>
    {
        public CreateNewYarnCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.Tags).NotEmpty();

            RuleFor(command => command.MaterialTypeDocument.Id).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Code).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Name).NotEmpty();

            RuleFor(command => command.RingDocument.Code).NotEmpty();
            RuleFor(command => command.RingDocument.Number).NotEmpty();
        }
    }
}
