using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class DailyOperationWarpingReadModel : ReadModelBase
    {
        public Guid ConstructionId { get; internal set; }
        public Guid MaterialTypeId { get; internal set; }
        public int AmountOfCones { get; internal set; }
        public string ColourOfCone { get; internal set; }
        public DateTimeOffset DateTimeOperation { get; internal set; }
        public Guid OperatorId { get; internal set; }
        public List<DailyOperationWarpingHistory> 
            DailyOperationWarpingDetailHistory { get; internal set; }

        public DailyOperationWarpingReadModel(Guid identity) 
            : base(identity) { }

        public List<DailyOperationWarpingBeamProduct> DailyOperationWarpingBeamProducts { get; internal set; }
    }
}
