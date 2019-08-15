using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.StockCard.Events.Sizing
{
    public class MoveOutBeamStockSizingEvent : BeamStockEvent
    {

    }

    public class MoveOutBeamStockSizingEventValidator
      : AbstractValidator<MoveOutBeamStockSizingEvent>
    {
        public MoveOutBeamStockSizingEventValidator()
        {
            RuleFor(command => command.StockNumber).NotEmpty();
            RuleFor(command => command.DailyOperationId.Value).NotEmpty();
            RuleFor(command => command.BeamId.Value).NotEmpty();
            RuleFor(command => command.DateTimeOperation).NotNull();
        }
    }
}
