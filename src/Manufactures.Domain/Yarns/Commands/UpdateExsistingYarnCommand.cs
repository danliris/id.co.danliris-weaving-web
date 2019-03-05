using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Yarns.Commands
{
    public class UpdateExsistingYarnCommand : ICommand<YarnDocument>
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; private set; }

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

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateExsistingYarnCommandValidator : AbstractValidator<UpdateExsistingYarnCommand>
    {
        public UpdateExsistingYarnCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.Tags).NotEmpty();
            RuleFor(command => command.MaterialTypeId).NotEmpty();
            RuleFor(command => command.YarnNumberId).NotEmpty();
        }
    }
}
