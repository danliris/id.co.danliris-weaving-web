using FluentValidation;

namespace Manufactures.Domain.StockCard.Events.Warping
{
    public class MoveOutBeamStockWarpingEvent : BeamStockEvent
    {
    }

    public class MoveOutBeamStockWarpingEventValidator
       : AbstractValidator<MoveOutBeamStockWarpingEvent>
    {
        public MoveOutBeamStockWarpingEventValidator()
        {
            RuleFor(command => command.StockNumber).NotEmpty();
            RuleFor(command => command.DailyOperationId.Value).NotEmpty();
            RuleFor(command => command.BeamId.Value).NotEmpty();
            RuleFor(command => command.DateTimeOperation).NotNull();
        }
    }
}
