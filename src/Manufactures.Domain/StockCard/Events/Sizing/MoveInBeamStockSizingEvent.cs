using FluentValidation;

namespace Manufactures.Domain.StockCard.Events.Sizing
{
    public class MoveInBeamStockSizingEvent : BeamStockEvent
    {
    }

    public class AddBeamStockSizingEventEventValidator
       : AbstractValidator<MoveInBeamStockSizingEvent>
    {
        public AddBeamStockSizingEventEventValidator()
        {
            RuleFor(command => command.StockNumber).NotEmpty();
            RuleFor(command => command.DailyOperationId.Value).NotEmpty();
            RuleFor(command => command.BeamId.Value).NotEmpty();
            RuleFor(command => command.DateTimeOperation).NotNull();
        }
    }
}
