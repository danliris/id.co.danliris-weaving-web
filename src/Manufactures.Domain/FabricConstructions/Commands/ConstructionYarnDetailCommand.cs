using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.FabricConstructions.Commands
{
    public class ConstructionYarnDetailCommand
    {
        public Guid Id { get; private set; }
        public Guid YarnId { get; private set; }
        public double Quantity { get; private set; }
        public string Information { get; private set; }
        public string Type { get; private set; }
        public Guid FabricConstructionDocumentId { get; private set; }
    }

    public class ConstructionYarnDetailCommandValidator : AbstractValidator<ConstructionYarnDetailCommand>
    {
        public ConstructionYarnDetailCommandValidator()
        {
            RuleFor(command => command.YarnId).NotEmpty();
            RuleFor(command => command.Quantity).NotEmpty();
            RuleFor(command => command.Type);
            RuleFor(command => command.FabricConstructionDocumentId);
        }
    }
}
