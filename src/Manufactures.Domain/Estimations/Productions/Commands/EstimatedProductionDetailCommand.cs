using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class EstimatedProductionDetailCommand
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ConstructionId { get; private set; }
        public double GradeA { get; private set; }
        public double GradeB { get; private set; }
        public double GradeC { get; private set; }
        public double GradeD { get; private set; }
        public Guid EstimatedProductionDocumentId { get; private set; }
    }

    public class EstimatedProductionDetailCommandValidator : AbstractValidator<EstimatedProductionDetailCommand>
    {
        public EstimatedProductionDetailCommandValidator()
        {
            RuleFor(command => command.OrderId).NotEmpty();
            RuleFor(command => command.ConstructionId).NotEmpty();
            RuleFor(command => command.GradeA).NotEmpty();
            RuleFor(command => command.GradeB).NotEmpty();
            RuleFor(command => command.GradeC).NotEmpty();
            RuleFor(command => command.EstimatedProductionDocumentId).NotEmpty();
        }
    }
}
