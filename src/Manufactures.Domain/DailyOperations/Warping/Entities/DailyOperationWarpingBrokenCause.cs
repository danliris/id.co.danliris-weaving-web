using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingBrokenCause : EntityBase<DailyOperationWarpingBrokenCause>
    {
        public Guid BrokenCauseId { get; private set; }
        public int TotalBroken { get; private set; }
        public Guid DailyOperationWarpingBeamProductId { get; set; }
        public DailyOperationWarpingBeamProduct DailyOperationWarpingBeamProduct { get; set; }

        public DailyOperationWarpingBrokenCause(Guid identity) : base(identity) { }

        public DailyOperationWarpingBrokenCause(Guid identity,
                                                BrokenCauseId brokenCauseId, 
                                                int totalBroken) : base(identity)
        {
            Identity = identity;
            BrokenCauseId = brokenCauseId.Value;
            TotalBroken = totalBroken;
        }

        public void SetBrokenCauseId(Guid value)
        {
            if (!BrokenCauseId.Equals(value))
            {
                BrokenCauseId = value;

                MarkModified();
            }
        }

        public void SetTotalBroken(int value)
        {
            if (!TotalBroken.Equals(value))
            {
                TotalBroken = value;

                MarkModified();
            }
        }

        protected override DailyOperationWarpingBrokenCause GetEntity()
        {
            return this;
        }
    }
}
