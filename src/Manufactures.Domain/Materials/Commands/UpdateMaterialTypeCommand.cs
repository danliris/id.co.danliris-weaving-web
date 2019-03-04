using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Materials.Commands
{
    public class UpdateMaterialTypeCommand : ICommand<MaterialTypeDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "RingDocuments")]
        public List<YarnNumberValueObject> RingDocuments { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateMaterialTypeCommadValidator : AbstractValidator<UpdateMaterialTypeCommand>
    {
        public UpdateMaterialTypeCommadValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
        }
    }
}
