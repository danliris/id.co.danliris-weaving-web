using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Yarns.Commands
{
    public class CreateNewYarnCommand : ICommand<YarnDocument>
    {
        [JsonProperty(propertyName: "Code")]
        public string Code { get; set; }

        [JsonProperty(propertyName: "Name")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "Tags")]
        public string Tags { get; set; }

        [JsonProperty(propertyName: "MaterialTypeId")]
        public string MaterialTypeId { get; set; }

        [JsonProperty(propertyName: "YarnNumberId")]
        public string YarnNumberId { get; set; }
    }

    public class CreateNewYarnCommandValidator : AbstractValidator<CreateNewYarnCommand>
    {
        public CreateNewYarnCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.Tags).NotEmpty();
            RuleFor(command => command.MaterialTypeId).NotEmpty();
            RuleFor(command => command.YarnNumberId).NotEmpty();
        }
    }
}
