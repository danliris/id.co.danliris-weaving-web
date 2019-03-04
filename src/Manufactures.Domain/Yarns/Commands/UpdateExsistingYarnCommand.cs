using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Yarns.Commands
{
    public class UpdateExsistingYarnCommand : ICommand<YarnDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Tags")]
        public string Tags { get; set; }

        [JsonProperty(PropertyName = "MaterialTypeDocument")]
        public MaterialTypeValueObject MaterialTypeDocument { get; set; }

        [JsonProperty(PropertyName = "RingDocument")]
        public YarnNumberValueObject RingDocument { get; set; }

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

            RuleFor(command => command.MaterialTypeDocument.Id).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Code).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Name).NotEmpty();

            RuleFor(command => command.RingDocument.Code).NotEmpty();
            RuleFor(command => command.RingDocument.Number).NotEmpty();
        }
    }
}
