using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomBeamHistory : AggregateRoot<DailyOperationLoomBeamHistory, DailyOperationLoomBeamHistoryReadModel>
    {
        public string BeamNumber { get; private set; }

        public string MachineNumber { get; private set; }

        public OperatorId OperatorDocumentId { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public ShiftId ShiftDocumentId { get; private set; }

        public int? WarpBrokenThreads { get; private set; }

        public int? WeftBrokenThreads { get; private set; }

        public int? LenoBrokenThreads { get; private set; }

        public string ReprocessTo { get; private set; }

        public string Information { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomBeamHistory(Guid identity, string beamNumber, string machineNumber, OperatorId operatorDocumentId, DateTimeOffset dateTimeMachine, ShiftId shiftDocumentId, string machineStatus, Guid dailyOperationLoomDocumentId) : base(identity)
        {
            MarkTransient();

            Identity = identity;
            BeamNumber = beamNumber;
            MachineNumber = machineNumber;
            OperatorDocumentId = operatorDocumentId;
            DateTimeMachine = dateTimeMachine;
            ShiftDocumentId = shiftDocumentId;
            MachineStatus = machineStatus;
            DailyOperationLoomDocumentId = dailyOperationLoomDocumentId;

            ReadModel = new DailyOperationLoomBeamHistoryReadModel(Identity)
            {
                BeamNumber = BeamNumber,
                MachineNumber = MachineNumber,
                OperatorDocumentId = OperatorDocumentId.Value,
                DateTimeMachine = DateTimeMachine,
                ShiftDocumentId = ShiftDocumentId.Value,
                MachineStatus = MachineStatus,
                DailyOperationLoomDocumentId = DailyOperationLoomDocumentId
            };

            ReadModel.AddDomainEvent(new OnAddDailyOperationLoomBeamHistory(Identity));
        }

        public DailyOperationLoomBeamHistory(DailyOperationLoomBeamHistoryReadModel readModel) : base(readModel)
        {
            BeamNumber = readModel.BeamNumber;
            MachineNumber = readModel.MachineNumber;
            OperatorDocumentId = new OperatorId(readModel.OperatorDocumentId);
            DateTimeMachine = readModel.DateTimeMachine;
            ShiftDocumentId = new ShiftId(readModel.ShiftDocumentId);
            WarpBrokenThreads = readModel.WarpBrokenThreads;
            WeftBrokenThreads = readModel.WeftBrokenThreads;
            LenoBrokenThreads = readModel.LenoBrokenThreads;
            ReprocessTo = readModel.ReprocessTo;
            Information = readModel.Information;
            MachineStatus = readModel.MachineStatus;
            DailyOperationLoomDocumentId = readModel.DailyOperationLoomDocumentId;
        }

        protected override DailyOperationLoomBeamHistory GetEntity()
        {
            return this;
        }


        public void SetBeamNumber(string newBeamNumber)
        {
            if (BeamNumber != newBeamNumber)
            {
                BeamNumber = newBeamNumber;
                ReadModel.BeamNumber = BeamNumber;
                MarkModified();
            }

        }

        public void SetMachineNumber(string newMachineNumber)
        {
            if (MachineNumber != newMachineNumber)
            {
                MachineNumber = newMachineNumber;
                ReadModel.MachineNumber = MachineNumber;
                MarkModified();
            }

        }

        public void SetOperatorDocumentId(OperatorId newOperatorDocumentId)
        {
            if(OperatorDocumentId != newOperatorDocumentId)
            {
                OperatorDocumentId = newOperatorDocumentId;
                ReadModel.OperatorDocumentId = OperatorDocumentId.Value;
                MarkModified();
            }
        }

        public void SetDateTimeMachine(DateTimeOffset newDateTimeMachine)
        {
            if(DateTimeMachine != newDateTimeMachine)
            {
                DateTimeMachine = newDateTimeMachine;
                ReadModel.DateTimeMachine = DateTimeMachine;
                MarkModified();
            }
        }

        public void SetShiftDocumentId(ShiftId newShiftDocumentId)
        {
            if(ShiftDocumentId != newShiftDocumentId)
            {
                ShiftDocumentId = newShiftDocumentId;
                ReadModel.ShiftDocumentId = newShiftDocumentId.Value;
                MarkModified();
            }
        }

        public void SetWarpBrokenThreads(int newWarpBrokenThreads)
        {
            if(WarpBrokenThreads.HasValue && WarpBrokenThreads.Value != newWarpBrokenThreads)
            {
                WarpBrokenThreads = newWarpBrokenThreads;
                ReadModel.WarpBrokenThreads = WarpBrokenThreads;
                MarkModified();
            }
        }

        public void SetWeftBrokenThreads(int newWeftBrokenThreads)
        {
            if (WeftBrokenThreads.HasValue && WeftBrokenThreads.Value != newWeftBrokenThreads)
            {
                WeftBrokenThreads = newWeftBrokenThreads;
                ReadModel.WeftBrokenThreads = WeftBrokenThreads;
                MarkModified();
            }
        }

        public void SetLenoBrokenThreads(int newLenoBrokenThreads)
        {
            if (LenoBrokenThreads.HasValue && LenoBrokenThreads.Value != newLenoBrokenThreads)
            {
                LenoBrokenThreads = newLenoBrokenThreads;
                ReadModel.LenoBrokenThreads = LenoBrokenThreads;
                MarkModified();
            }
        }

        public void SetReprocessTo(string newReprocessTo)
        {
            if(ReprocessTo != newReprocessTo)
            {
                ReprocessTo = newReprocessTo;
                ReadModel.ReprocessTo = ReprocessTo;
                MarkModified();

            }
        }

        public void SetInformation(string newInformation)
        {
            if(Information != newInformation)
            {
                Information = newInformation;
                ReadModel.Information = Information;
                MarkModified();
            }
        }

        public void SetMachineStatus(string newMachineStatus)
        {
            if(MachineStatus != newMachineStatus)
            {
                MachineStatus = newMachineStatus;
                ReadModel.MachineStatus = MachineStatus;
                MarkModified();
            }
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }
    }
}
