using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.Materials.Commands
{
    public class PlaceMaterialTypeCommand : ICommand<MaterialTypeDocument>
    {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "RingDocuments")]
        public List<RingDocumentValueObject> RingDocuments { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
    }

    public class PlaceMaterialTypeCommandValidator : AbstractValidator<PlaceMaterialTypeCommand>
    {
        public PlaceMaterialTypeCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
        }
    }
}
