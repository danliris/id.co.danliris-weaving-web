using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.FabricConstructions.Commands
{
    public class ConstructionYarnDetailCommand
    {
        public Guid Id { get; set; }
        public Guid YarnId { get; set; }
        public double Quantity { get; set; }
        public string Information { get; set; }
        public string Type { get; set; }
        public Guid FabricConstructionDocumentId { get; set; }
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
