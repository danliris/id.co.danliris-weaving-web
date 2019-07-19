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
                                            Guid beamOperatorId,
                                            DateTimeOffset datetimeOperation,
                                            string operationStatus)
            : base(identity)
        {
            Identity = identity;
            BeamOperatorId = beamOperatorId;
            DateTimeOperation = datetimeOperation;
            OperationStatus = operationStatus;
        }

        public void SetBeamNumber(string value)
        {
            if(!BeamNumber.Equals(value))
            {
                BeamNumber = value;

                MarkModified();
            }
        }

        public void SetInformation(string value)
        {
            if(!Information.Equals(value))
            {
                Information = value;

                MarkModified();
            }
        }

        protected override DailyOperationWarpingHistory GetEntity()
        {
            return this;
        }
    }
}
