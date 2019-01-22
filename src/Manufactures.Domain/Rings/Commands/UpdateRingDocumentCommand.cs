using FluentValidation;
using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Rings.Commands
{
    public class UpdateRingDocumentCommand : ICommand<RingDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateRingDocumentCommandValidator : AbstractValidator<UpdateRingDocumentCommand>
    {
        public UpdateRingDocumentCommandValidator()
        {
            RuleFor(command => command.Code).NotNull();
            RuleFor(command => command.Name).NotNull();
            RuleFor(command => command.Description).NotNull();
        }
    }
}
