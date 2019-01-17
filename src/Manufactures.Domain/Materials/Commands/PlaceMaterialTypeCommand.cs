using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Materials;

namespace Manufactures.Domain.Materials.Commands
{
    public class PlaceMaterialTypeCommand : ICommand<MaterialType>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class PlaceMaterialTypeCommandValidator : AbstractValidator<PlaceMaterialTypeCommand>
    {
        public PlaceMaterialTypeCommandValidator()
        {
            RuleFor(command => command.Code).NotNull();
            RuleFor(command => command.Name).NotNull();
            RuleFor(command => command.Description).NotNull();
        }
    }
}
