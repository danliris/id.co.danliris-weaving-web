using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Entities
{
    public class DailyOperationReachingDetail : EntityBase<DailyOperationReachingDetail>
    {
        public Guid OperatorDocumentId { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public Guid ShiftDocumentId { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationReachingDocumentId { get; set; }

        public DailyOperationReachingReadModel DailyOperationReachingDocument { get; set; }

        public DailyOperationReachingDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationReachingDetail(Guid identity,
                                            OperatorId operatorDocumentId,
                                            DateTimeOffset dateTimeMachine,
                                            ShiftId shiftDocumentId,
                                            string machineStatus) : base(identity)
        {
            OperatorDocumentId = operatorDocumentId.Value;
            DateTimeMachine = dateTimeMachine;
            ShiftDocumentId = shiftDocumentId.Value;
            MachineStatus = machineStatus;
        }

        public void SetOperatorDocumentId(OperatorId operatorDocumentId)
        {
            if (!OperatorDocumentId.Equals(operatorDocumentId.Value))
            {
                OperatorDocumentId = operatorDocumentId.Value;
                MarkModified();
            }
        }

        public void SetDateTimeMachine(DateTimeOffset dateTimeMachine)
        {
            DateTimeMachine = dateTimeMachine;
            MarkModified();
        }

        public void SetShiftId(ShiftId shiftDocumentId)
        {
            ShiftDocumentId = shiftDocumentId.Value;
            MarkModified();
        }

        public void SetMachineStatus(string machineStatus)
        {
            MachineStatus = machineStatus;
            MarkModified();
        }

        protected override DailyOperationReachingDetail GetEntity()
        {
            return this;
        }
    }
}
