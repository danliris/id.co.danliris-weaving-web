using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

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
        public Guid? FabricSpecificationId { get; internal set; }
        public string WeavingUnit { get; internal set; }
        public string UserId { get; internal set; }
    }
}
