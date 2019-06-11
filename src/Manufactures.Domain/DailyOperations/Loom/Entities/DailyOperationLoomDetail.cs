using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
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
        public DateTimeOffset DateTimeOperation { get; private set; }
        public string OperationStatus { get; private set; }
        public bool IsUp { get; private set; }
        public bool IsDown { get; private set; }
        public Guid DailyOperationLoomDocumentId { get; set; }
        public DailyOperationLoomReadModel DailyOperationLoomDocument { get; set; }

        public DailyOperationLoomDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationLoomDetail(Guid identity,
                                        ShiftId shiftId,
                                        OperatorId beamOperatorId,
                                        DateTimeOffset dateTimeOperation,
                                        string operationStatus,
                                        bool isUp,
                                        bool isDown)
            : base(identity)
        {
            Identity = identity;
            ShiftId = shiftId.Value;
            BeamOperatorId = beamOperatorId.Value;
            DateTimeOperation = dateTimeOperation;
            OperationStatus = operationStatus;
            IsUp = isUp;
            IsDown = isDown;
        }

        public void AddWarpOrigin(string warpOrigin)
        {
            WarpOrigin = warpOrigin;
        }

        public void AddWeftOrigin(string weftOrigin)
        {
            WeftOrigin = weftOrigin;
        }

        protected override DailyOperationLoomDetail GetEntity()
        {
            return this;
        }
    }
}
