using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Orders.ReadModels
{
    public class OrderReadModel : ReadModelBase
    {
        public string OrderNumber { get; internal set; }
        public DateTime Period { get; internal set; }
        public Guid ConstructionDocumentId { get; internal set; }
        public string YarnType { get; internal set; }
        public Guid WarpOriginId { get; internal set; }
        public double WarpCompositionPoly { get; internal set; }
        public double WarpCompositionCotton { get; internal set; }
        public double WarpCompositionOthers { get; internal set; }
        public Guid WeftOriginId { get; internal set; }
        public double WeftCompositionPoly { get; internal set; }
        public double WeftCompositionCotton { get; internal set; }
        public double WeftCompositionOthers { get; internal set; }
        public double AllGrade { get; internal set; }
        public int UnitId { get; internal set; }
        public string OrderStatus { get; internal set; }

        public OrderReadModel(Guid identity) : base(identity) { }
    }
}
