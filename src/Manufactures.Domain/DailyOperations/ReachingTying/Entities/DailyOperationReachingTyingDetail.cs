using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.ReachingTying.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.Entities
{
    public class DailyOperationReachingTyingDetail : EntityBase<DailyOperationReachingTyingDetail>
    {
        public Guid OperatorDocumentId { get; private set; }

        public int YarnStrandsProcessed { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public Guid ShiftDocumentId { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationReachingTyingDocumentId { get; set; }

        public DailyOperationReachingTyingReadModel DailyOperationReachingTyingDocument { get; set; }

        public DailyOperationReachingTyingDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationReachingTyingDetail(Guid identity,
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

        public DailyOperationReachingTyingDetail(Guid identity,
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

        protected override DailyOperationReachingTyingDetail GetEntity()
        {
            return this;
        }
    }
}
