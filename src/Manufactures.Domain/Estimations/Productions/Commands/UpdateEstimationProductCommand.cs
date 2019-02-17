using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class UpdateEstimationProductCommand : ICommand<EstimatedProductionDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public Period Period { get; set; }
        public WeavingUnit Unit { get; set; }
        public List<EstimationProductValueObject> EstimationProducts { get; private set; }
    }

    public class UpdateEstimationProductCommandValidator : AbstractValidator<UpdateEstimationProductCommand>
    {
        public UpdateEstimationProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.Period).NotEmpty();
            RuleFor(command => command.Unit).NotEmpty();
        }
    }
}
