using FluentValidation;
using Infrastructure.Domain.Commands;

namespace Manufactures.Domain.Suppliers.Commands
{
    public class PlaceNewSupplierCommand : ICommand<WeavingSupplierDocument>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CoreSupplierId { get; set; }
    }

    public class PlaceNewSupplierCommandValidator : AbstractValidator<PlaceNewSupplierCommand>
    {
        public PlaceNewSupplierCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.CoreSupplierId).NotEmpty();
        }
    }
}
