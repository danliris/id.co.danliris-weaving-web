using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class DailyOperationWarpingReadModel : ReadModelBase
    {
        public DailyOperationWarpingReadModel(Guid identity) 
            : base(identity) { }

        public Guid ConstructionId { get; internal set; }
        public Guid MaterialTypeId { get; internal set; }
        public int AmountOfCones { get; internal set; }
        public string ColourOfCone { get; internal set; }
        public DateTimeOffset DateTimeOperation { get; internal set; }
        public Guid OperatorId { get; internal set; }
    }
}
