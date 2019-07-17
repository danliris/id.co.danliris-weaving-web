using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingHistory
        : EntityBase<DailyOperationWarpingHistory>
    {
        public Guid ShiftId { get; private set; }
        public string BeamNumber { get; private set; }
        public Guid BeamOperatorId { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public string Information { get; private set; }
        public string OperationStatus { get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }
        public DailyOperationWarpingReadModel DailyOperationWarpingDocument { get; set; }

        public DailyOperationWarpingHistory(Guid identity) : base(identity) { }

        public DailyOperationWarpingHistory(Guid identity,
                                                   string beamNumber,
                                                   Guid beamOperatorId,
                                                   DateTimeOffset datetimeOperation,
                                                   string information,
                                                   string operationStatus)
            : base(identity)
        {
            Identity = identity;
            BeamNumber = beamNumber;
            BeamOperatorId = beamOperatorId;
            DateTimeOperation = datetimeOperation;
            Information = information;
            OperationStatus = operationStatus;
        }

        protected override DailyOperationWarpingHistory GetEntity()
        {
            return this;
        }
    }
}
