using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Operators.Commands
{
    public class AddOperatorCommand : ICommand<OperatorDocument>
    {
        [JsonProperty(propertyName: "CoreAccount")]
        public CoreAccountCommand CoreAccount { get; set; }

        [JsonProperty(propertyName: "UnitId")]
        public UnitIdCommand UnitId { get; set; }

        [JsonProperty(propertyName: "Group")]
        public string Group { get; set; }

        [JsonProperty(propertyName: "Id")]
        public string Assignment { get; set; }

        [JsonProperty(propertyName: "Type")]
        public string Type { get; set; }
    }

    public class AddOperatorCommandValidator : AbstractValidator<AddOperatorCommand>
    {
        public AddOperatorCommandValidator()
        {
            RuleFor(command => command.CoreAccount.MongoId).NotEmpty();
            RuleFor(command => command.UnitId.Id).NotEmpty();
            RuleFor(command => command.Group).NotEmpty();
            RuleFor(command => command.Assignment).NotEmpty();
            RuleFor(command => command.Type).NotEmpty();
        }
    }
}
