using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BrokenCauses.Warping.ReadModels
{
    public class WarpingBrokenCauseReadModel : ReadModelBase
    {
        public string WarpingBrokenCauseName { get; internal set; }
        public string Information { get; internal set; }
        public bool IsOthers { get; internal set; }
        public WarpingBrokenCauseReadModel(Guid identity) : base(identity)
        {
        }
    }
}
