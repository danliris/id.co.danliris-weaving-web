using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class AddNewEstimationCommand : ICommand<EstimatedProductionDocument>
    {
        public Period Period { get; set; }
        public WeavingUnit Unit { get; set; }
        public double TotalEstimationOrder { get; set; }
        public List<EstimationProductValueObject> EstimationProducts { get; private set; }
    }

    public class AddNewEstimationCommandValidator : AbstractValidator<AddNewEstimationCommand>
    {
        public AddNewEstimationCommandValidator()
        {
            RuleFor(command => command.Period).NotEmpty();
            RuleFor(command => command.Unit).NotEmpty();
            RuleFor(command => command.TotalEstimationOrder).NotEmpty();
            RuleForEach(command => command.EstimationProducts).NotEmpty();
        }
    }
}
