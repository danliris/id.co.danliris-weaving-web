using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Reaching.Entities
{
    public class DailyOperationReachingHistory : EntityBase<DailyOperationReachingHistory>
    {
        public Guid OperatorDocumentId { get; private set; }

        public int YarnStrandsProcessed { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public Guid ShiftDocumentId { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationReachingDocumentId { get; set; }

        public DailyOperationReachingReadModel DailyOperationReachingDocument { get; set; }

        public DailyOperationReachingHistory(Guid identity) : base(identity)
        {
        }

        public DailyOperationReachingHistory(Guid identity,
                                                 OperatorId operatorDocumentId,
                                                 int yarnStrandsProcessed,
                                                 DateTimeOffset dateTimeMachine,
                                                 ShiftId shiftDocumentId,
                                                 string machineStatus) : base(identity)
        {
            OperatorDocumentId = operatorDocumentId.Value;
            YarnStrandsProcessed = yarnStrandsProcessed;
            DateTimeMachine = dateTimeMachine;
            ShiftDocumentId = shiftDocumentId.Value;
            MachineStatus = machineStatus;
        }

        public DailyOperationReachingHistory(Guid identity,
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

        public void SetYarnStrandsProcessed(int yarnStrandsProcessed)
        {
            YarnStrandsProcessed = yarnStrandsProcessed;
            MarkModified();
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

        protected override DailyOperationReachingHistory GetEntity()
        {
            return this;
        }
    }
}
