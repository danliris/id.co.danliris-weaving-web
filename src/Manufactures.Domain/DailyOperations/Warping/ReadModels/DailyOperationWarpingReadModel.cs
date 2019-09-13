using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class DailyOperationWarpingReadModel : ReadModelBase
    {
        public Guid OrderDocumentId { get; internal set; }
        public Guid MaterialTypeId { get; internal set; }
        public int AmountOfCones { get; internal set; }
        public string ColourOfCone { get; internal set; }
        public DateTimeOffset DateTimeOperation { get; internal set; }
        public string OperationStatus { get; internal set; }
        public List<DailyOperationWarpingHistory> WarpingHistories { get; internal set; }
        public List<DailyOperationWarpingBeamProduct> WarpingBeamProducts { get; internal set; }

        public DailyOperationWarpingReadModel(Guid identity) : base(identity) { }
    }
}
