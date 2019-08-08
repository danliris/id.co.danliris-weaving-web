using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.StockCard.ReadModels
{
    public class StockCardReadModel : ReadModelBase
    {
        private StockCardReadModel readModel;

        public StockCardReadModel(Guid identity) : base(identity) { }

        public string StockNumber { get; internal set; }
        public Guid DailyOperationId { get; internal set; }
        public DateTimeOffset DateTimeOperation { get; internal set; }
        public Guid BeamId { get; internal set; }
        public double? Length { get; internal set; }
        public double? YarnStrands { get; internal set; }
        public bool IsAvailable { get; internal set; }
        
    }
}
