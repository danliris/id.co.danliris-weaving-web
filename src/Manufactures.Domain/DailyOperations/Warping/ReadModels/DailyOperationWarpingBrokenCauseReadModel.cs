using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class DailyOperationWarpingBrokenCauseReadModel : ReadModelBase
    {
        public Guid BrokenCauseId { get; internal set; }
        public int TotalBroken { get; internal set; }
        public Guid DailyOperationWarpingBeamProductId { get; set; }

        public DailyOperationWarpingBrokenCauseReadModel(Guid identity) : base(identity)
        {
        }
    }
}
