using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class DailyOperationWarpingDocumentReadModel : ReadModelBase
    {
        public Guid OrderDocumentId { get; internal set; }
        public int AmountOfCones { get; internal set; }
        public int BeamProductResult { get; internal set; }
        public DateTimeOffset DateTimeOperation { get; internal set; }
        public string OperationStatus { get; internal set; }

        public DailyOperationWarpingDocumentReadModel(Guid identity) : base(identity) { }
    }
}
