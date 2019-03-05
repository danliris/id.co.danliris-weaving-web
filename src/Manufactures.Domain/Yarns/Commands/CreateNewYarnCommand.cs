using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.Yarns.Commands
{
    public class CreateNewYarnCommand : ICommand<YarnDocument>
    {
        [JsonProperty]
        public string Code { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Tags { get; set; }

        [JsonProperty]
        public MaterialTypeId MaterialTypeId { get; set; }

        [JsonProperty]
        public YarnNumberId YarnNumberId { get; set; }
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
