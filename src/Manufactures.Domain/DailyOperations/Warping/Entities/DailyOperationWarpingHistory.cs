using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingHistory : EntityBase<DailyOperationWarpingHistory>
    {
        public Guid ShiftDocumentId { get; private set; }
        public Guid OperatorDocumentId { get; private set; }
        public DateTimeOffset DateTimeMachine { get; private set; }
        public string MachineStatus { get; private set; }
        public string Information { get; private set; }
        public Guid WarpingBeamId { get; private set; }
        public double WarpingBeamLengthPerOperator { get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }
        public DailyOperationWarpingReadModel DailyOperationWarpingDocument { get; set; }

        public DailyOperationWarpingHistory(Guid identity) : base(identity) { }

        //Constructor for Preparation Process
        public DailyOperationWarpingHistory(Guid identity,
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

        public void SetWarpingBeamId(BeamId warpingBeamId)
        {
            if (!WarpingBeamId.Equals(warpingBeamId))
            {
                WarpingBeamId = warpingBeamId.Value;
                MarkModified();
            }
        }

        public void SetWarpingBeamLengthPerOperator(double warpingBeamLengthPerOperator)
        {
            if (!WarpingBeamLengthPerOperator.Equals(warpingBeamLengthPerOperator))
            {
                WarpingBeamLengthPerOperator = warpingBeamLengthPerOperator;
                MarkModified();
            }
        }

        protected override DailyOperationWarpingHistory GetEntity()
        {
            return this;
        }
    }
}
