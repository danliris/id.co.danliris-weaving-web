using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Yarns.ValueObjects;

namespace Manufactures.Domain.Yarns.Commands
{
    public class CreateNewYarnCommand : ICommand<YarnDocument>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public CurrencyValueObject CoreCurrency { get; set; }
        public UomValueObject CoreUom { get; set; }
        public MaterialTypeDocumentValueObject MaterialTypeDocument { get; set; }
        public RingDocumentValueObject RingDocument { get; set; }
        public SupplierDocumentValueObject SupplierDocument { get; set; }
        public double Price { get; set; }
    }

    public class CreateNewYarnCommandValidator : AbstractValidator<CreateNewYarnCommand>
    {
        public CreateNewYarnCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.Description).NotEmpty();
            RuleFor(command => command.Tags).NotEmpty();

            RuleFor(command => command.CoreCurrency.Code).NotEmpty();
            RuleFor(command => command.CoreCurrency.Name).NotEmpty();

            RuleFor(command => command.CoreUom.Code).NotEmpty();
            RuleFor(command => command.CoreUom.Unit).NotEmpty();

            RuleFor(command => command.MaterialTypeDocument.Code).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Name).NotEmpty();

            RuleFor(command => command.RingDocument.Code).NotEmpty();
            RuleFor(command => command.RingDocument.Name).NotEmpty();

            RuleFor(command => command.SupplierDocument.Code).NotEmpty();
            RuleFor(command => command.SupplierDocument.Name).NotEmpty();

            RuleFor(command => command.Price).NotEmpty();
        }
    }
}
