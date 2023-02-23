using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.StockCard.ReadModels
{
    public class StockCardReadModel : ReadModelBase
    {
        public StockCardReadModel(Guid identity) : base(identity) { }

        public string StockNumber { get; internal set; }
        public Guid DailyOperationId { get; internal set; }
        public string BeamDocument { get; internal set; }
        public string StockStatus { get; internal set; }
        public bool Expired { get; internal set; }
        public Guid BeamId { get; internal set; }
    }
}
