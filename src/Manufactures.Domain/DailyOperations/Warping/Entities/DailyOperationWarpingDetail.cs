using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingDetail : EntityBase<DailyOperationWarpingDetail>
    {
        public Guid ShiftDocumentId { get; private set; }
        public Guid OperatorDocumentId { get; private set; }
        public DateTimeOffset DateTimeMachine { get; private set; }
        public string MachineStatus { get; private set; }
        public string Information { get; private set; }
        public string WarpingBeamNumber { get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }
        public DailyOperationWarpingReadModel DailyOperationWarpingDocument { get; set; }

        public DailyOperationWarpingDetail(Guid identity) : base(identity) { }

        public DailyOperationWarpingDetail(Guid identity,
                                           ShiftId shiftDocumentId,
                                           OperatorId operatorDocumentId,
                                           DateTimeOffset datetimeMachine,
                                           string machineStatus,
                                           string information,
                                           string warpingBeamNumber) : base(identity)
        {
            ShiftDocumentId = shiftDocumentId.Value;
            OperatorDocumentId = operatorDocumentId.Value;
            DateTimeMachine = datetimeMachine;
            MachineStatus = machineStatus;
            Information = information;
            WarpingBeamNumber = warpingBeamNumber;
        }

        public DailyOperationWarpingDetail(Guid identity,
                                           ShiftId shiftDocumentId,
                                           OperatorId operatorDocumentId,
                                           DateTimeOffset dateTimeMachine,
                                           string machineStatus) : base(identity)
        {
            ShiftDocumentId = shiftDocumentId.Value;
            OperatorDocumentId = operatorDocumentId.Value;
            DateTimeMachine = dateTimeMachine;
            MachineStatus = machineStatus;
        }

        public void SetShiftId(ShiftId shiftDocumentId)
        {
            ShiftDocumentId = shiftDocumentId.Value;
            MarkModified();
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

        public void SetMachineStatus(string machineStatus)
        {
            MachineStatus = machineStatus;
            MarkModified();
        }

        public void SetInformation(string information)
        {
            if (!Information.Equals(information))
            {
                Information = information;
                MarkModified();
            }
        }

        public void SetWarpingBeamNumber(string warpingBeamNumber)
        {
            if (!WarpingBeamNumber.Equals(warpingBeamNumber))
            {
                WarpingBeamNumber = warpingBeamNumber;
                MarkModified();
            }
        }

        protected override DailyOperationWarpingDetail GetEntity()
        {
            return this;
        }
    }
}
