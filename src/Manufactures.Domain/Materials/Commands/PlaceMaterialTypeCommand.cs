using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials;
using System.Collections.Generic;

namespace Manufactures.Domain.Materials.Commands
{
    public class PlaceMaterialTypeCommand : ICommand<MaterialTypeDocument>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<RingDocumentValueObject> RingDocuments { get; set; }
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
