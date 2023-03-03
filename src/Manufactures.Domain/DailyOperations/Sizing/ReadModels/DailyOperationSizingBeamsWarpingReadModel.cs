using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ReadModels
{
    public class DailyOperationSizingBeamsWarpingReadModel : ReadModelBase
    {
        public Guid BeamDocumentId { get; internal set; }
        public double YarnStrands { get; internal set; }
        public double EmptyWeight { get; internal set; }
        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingBeamsWarpingReadModel(Guid identity) : base(identity)
        {
        }
    }
}
