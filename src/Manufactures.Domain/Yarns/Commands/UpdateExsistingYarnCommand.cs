using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Yarns.ValueObjects;
using System;

namespace Manufactures.Domain.Yarns.Commands
{
    public class UpdateExsistingYarnCommand : ICommand<YarnDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public MaterialTypeDocumentValueObject MaterialTypeDocument { get; set; }
        public RingDocumentValueObject RingDocument { get; set; }
    }

    public class UpdateExsistingYarnCommandValidator : AbstractValidator<UpdateExsistingYarnCommand>
    {
        public UpdateExsistingYarnCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.Tags).NotEmpty();

            RuleFor(command => command.MaterialTypeDocument.Code).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Name).NotEmpty();

            RuleFor(command => command.RingDocument.Code).NotEmpty();
            RuleFor(command => command.RingDocument.Number).NotEmpty();
        }
    }
}
