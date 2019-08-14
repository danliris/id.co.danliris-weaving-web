using System;
using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.StockCard.Events.Warping
{
    public class MoveInBeamStockWarpingEvent : BeamStockEvent
    {
    }
    public class AddBeamStockWarpingEventValidator
       : AbstractValidator<MoveInBeamStockWarpingEvent>
    {
        public AddBeamStockWarpingEventValidator()
        {
            RuleFor(command => command.StockNumber).NotEmpty();
            RuleFor(command => command.DailyOperationId.Value).NotEmpty();
            RuleFor(command => command.BeamId.Value).NotEmpty();
            RuleFor(command => command.DateTimeOperation).NotNull();
        }
    }
}
