using FluentValidation;
using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Suppliers.Commands
{
    public class UpdateExsistingSupplierCommand : ICommand<WeavingSupplierDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CoreSupplierId { get; set; }
    }

    public class UpdateExsistingSupplierCommandValidator : AbstractValidator<WeavingSupplierDocument>
    {
        public UpdateExsistingSupplierCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.CoreSupplierId).NotEmpty();
        }
    }
}
