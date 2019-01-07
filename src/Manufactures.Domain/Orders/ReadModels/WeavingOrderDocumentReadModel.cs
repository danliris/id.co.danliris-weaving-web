using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Orders.ReadModels
{
    public class WeavingOrderDocumentReadModel : ReadModelBase
    {
        public WeavingOrderDocumentReadModel(Guid identity) : base(identity) { }

        public string OrderNumber { get; internal set; }
        public DateTimeOffset DateOrdered { get; internal set; }
        public string WarpOrigin { get; internal set; }
        public string WeftOrigin { get; internal set; }
        public int WholeGrade { get; internal set; }
        public string YarnType { get; internal set; }
        public string Period { get; internal set; }
        public string Composition { get; internal set; }
        public string FabricSpecification { get; internal set; }
        public string WeavingUnit { get; internal set; }
        public string UserId { get; internal set; }
    }
}
