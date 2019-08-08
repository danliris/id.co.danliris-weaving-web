using System;
using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.StockCard.Events.Warping
{
    public class AddBeamStockWarpingEvent : AddBeamStockEvent
    {
        public double?  Length { get; set; }

        public double? YarnStrands { get; set; }
    }

    public class AddBeamStockWarpingEventValidator
        : AbstractValidator<AddBeamStockWarpingEvent>
    {
        public AddBeamStockWarpingEventValidator()
        {
            RuleFor(command => command.StockNumber).NotEmpty();
            RuleFor(command => command.DailyOperationId.Value).NotEmpty();
            RuleFor(command => command.DateTimeOperation).NotNull();
            RuleFor(command => command.BeamId.Value).NotEmpty();
        }
    }
}
