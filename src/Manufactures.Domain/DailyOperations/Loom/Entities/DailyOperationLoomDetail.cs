using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomDetail
        : EntityBase<DailyOperationLoomDetail>
    {
        public Guid ShiftId { get; private set; }
        public Guid BeamOperatorId { get; private set; }
        public string WarpOrigin { get; private set; }
        public string WeftOrigin { get; private set; }
        public string DailyOperationLoomHistory { get; private set; }
        public Guid DailyOperationLoomDocumentId { get; set; }
        public DailyOperationLoomReadModel DailyOperationLoomDocument { get; set; }

        public DailyOperationLoomDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationLoomDetail(Guid identity, 
                                        ShiftId shiftId, 
                                        OperatorId beamOperatorId, 
                                        string warpOrigin, 
                                        string weftOrigin, 
                                        DailyOperationLoomHistory dailyOperationLoomHistory)
            : base(identity)
        {
            Identity = identity;
            ShiftId = shiftId.Value;
            BeamOperatorId = beamOperatorId.Value;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
            DailyOperationLoomHistory = dailyOperationLoomHistory.Serialize();
        }

        protected override DailyOperationLoomDetail GetEntity()
        {
            return this;
        }
    }
}
