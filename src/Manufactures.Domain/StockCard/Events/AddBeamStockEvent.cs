using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.StockCard.Events
{
    public class AddBeamStockEvent : IManufactureEvent
    {
        public BeamId BeamId { get; set; }

        public string StockNumber { get; set; }

        public DailyOperationId DailyOperationId { get; set; }

        public DateTimeOffset DateTimeOperation { get; set; }

    }
}
