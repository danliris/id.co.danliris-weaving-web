using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Operators.Commands
{
    public class UpdateOperatorCommand : ICommand<OperatorDocument>
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(propertyName: "CoreAccount")]
        public CoreAccountCommand CoreAccount { get; set; }

        [JsonProperty(propertyName: "Group")]
        public string Group { get; set; }

        [JsonProperty(propertyName: "Id")]
        public string Assignment { get; set; }

        [JsonProperty(propertyName: "Type")]
        public string Type { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateOperatorCommandValidator : AbstractValidator<UpdateOperatorCommand>
    {
        public UpdateOperatorCommandValidator()
        {
            RuleFor(command => command.CoreAccount.MongoId).NotEmpty();
            RuleFor(command => command.Group).NotEmpty();
            RuleFor(command => command.Assignment).NotEmpty();
            RuleFor(command => command.Type).NotEmpty();
        }
    }
}
