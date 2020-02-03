using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingBrokenCause : AggregateRoot<DailyOperationWarpingBrokenCause, DailyOperationWarpingBrokenCauseReadModel>
    {
        public Guid BrokenCauseId { get; private set; }
        public int TotalBroken { get; private set; }
        public Guid DailyOperationWarpingBeamProductId { get; set; }

        public DailyOperationWarpingBrokenCause(Guid identity) : base(identity) { }

        public DailyOperationWarpingBrokenCause(Guid identity,
                                                BrokenCauseId brokenCauseId,
                                                int totalBroken,
                                                Guid dailyOperationWarpingBeamProductId) : base(identity)
        {
            Identity = identity;
            BrokenCauseId = brokenCauseId.Value;
            TotalBroken = totalBroken;
            DailyOperationWarpingBeamProductId = dailyOperationWarpingBeamProductId;
            MarkTransient();
            ReadModel = new DailyOperationWarpingBrokenCauseReadModel(Identity)
            {
                BrokenCauseId = BrokenCauseId,
                TotalBroken = TotalBroken,
                DailyOperationWarpingBeamProductId = DailyOperationWarpingBeamProductId
            };
        }

        public DailyOperationWarpingBrokenCause(DailyOperationWarpingBrokenCauseReadModel readModel) : base(readModel)
        {
            BrokenCauseId = readModel.BrokenCauseId;
            TotalBroken = readModel.TotalBroken;
            DailyOperationWarpingBeamProductId = readModel.DailyOperationWarpingBeamProductId;
        }

        public void SetBrokenCauseId(Guid brokenCauseId)
        {
            Validator.ThrowIfNull(() => brokenCauseId);

            if (brokenCauseId != BrokenCauseId)
            {
                BrokenCauseId = brokenCauseId;
                ReadModel.BrokenCauseId = brokenCauseId;

                MarkModified();
            }
        }

        public void SetTotalBroken(int totalBroken)
        {
            Validator.ThrowIfNull(() => totalBroken);

            if (totalBroken != TotalBroken)
            {
                TotalBroken = totalBroken;
                ReadModel.TotalBroken = totalBroken;

                MarkModified();
            }
        }

        protected override DailyOperationWarpingBrokenCause GetEntity()
        {
            return this;
        }
    }
}
