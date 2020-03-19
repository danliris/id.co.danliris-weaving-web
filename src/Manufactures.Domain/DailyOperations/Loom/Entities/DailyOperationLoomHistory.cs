using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomHistory : AggregateRoot<DailyOperationLoomHistory, DailyOperationLoomHistoryReadModel>
    {
        public BeamId BeamDocumentId { get; private set; }

        public string BeamNumber { get; private set; }

        public MachineId TyingMachineId { get; private set; }

        public OperatorId TyingOperatorId { get; private set; }

        public MachineId LoomMachineId { get; private set; }

        public OperatorId LoomOperatorId { get; private set; }

        public double CounterPerOperator { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public ShiftId ShiftDocumentId { get; private set; }

        public string Information { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomHistory(Guid identity,
                                         BeamId beamDocumentId,
                                         string beamNumber,
                                         MachineId loomMachineId,
                                         OperatorId loomOperatorId,
                                         double counterPerOperator,
                                         DateTimeOffset dateTimeMachine,
                                         ShiftId shiftDocumentId,
                                         string machineStatus,
                                         Guid dailyOperationLoomDocumentId) : base(identity)
        {
            Identity = identity;
            BeamDocumentId = beamDocumentId;
            BeamNumber = beamNumber;
            LoomMachineId = loomMachineId;
            LoomOperatorId = loomOperatorId;
            CounterPerOperator = counterPerOperator;
            DateTimeMachine = dateTimeMachine;
            ShiftDocumentId = shiftDocumentId;
            MachineStatus = machineStatus;
            DailyOperationLoomDocumentId = dailyOperationLoomDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationLoomHistoryReadModel(Identity)
            {
                BeamDocumentId = BeamDocumentId.Value,
                BeamNumber = BeamNumber,
                TyingMachineId = TyingMachineId.Value,
                TyingOperatorId = TyingOperatorId.Value,
                LoomMachineId = LoomMachineId.Value,
                LoomOperatorId = LoomOperatorId.Value,
                CounterPerOperator = CounterPerOperator,
                DateTimeMachine = DateTimeMachine,
                ShiftDocumentId = ShiftDocumentId.Value,
                MachineStatus = MachineStatus,
                DailyOperationLoomDocumentId = DailyOperationLoomDocumentId
            };

            //ReadModel.AddDomainEvent(new OnAddDailyOperationLoomBeamHistory(Identity));
        }

        public DailyOperationLoomHistory(DailyOperationLoomHistoryReadModel readModel) : base(readModel)
        {
            BeamDocumentId = new BeamId(readModel.BeamDocumentId);
            BeamNumber = readModel.BeamNumber;
            TyingMachineId = new MachineId(readModel.TyingMachineId);
            TyingOperatorId = new OperatorId(readModel.TyingOperatorId);
            LoomMachineId = new MachineId(readModel.LoomMachineId);
            LoomOperatorId = new OperatorId(readModel.LoomOperatorId);
            CounterPerOperator = readModel.CounterPerOperator;
            DateTimeMachine = readModel.DateTimeMachine;
            ShiftDocumentId = new ShiftId(readModel.ShiftDocumentId);
            Information = readModel.Information;
            MachineStatus = readModel.MachineStatus;
            DailyOperationLoomDocumentId = readModel.DailyOperationLoomDocumentId;
        }

        public void SetBeamDocumentId(BeamId beamDocumentId)
        {
            Validator.ThrowIfNull(() => beamDocumentId);
            if (BeamDocumentId != beamDocumentId)
            {
                BeamDocumentId = beamDocumentId;
                ReadModel.BeamDocumentId = BeamDocumentId.Value;
                MarkModified();
            }
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

        public void SetTyingMachineId(MachineId tyingMachineId)
        {
            Validator.ThrowIfNull(() => tyingMachineId);
            if (tyingMachineId != TyingMachineId)
            {
                TyingMachineId = tyingMachineId;
                ReadModel.TyingMachineId = TyingMachineId.Value;

                MarkModified();
            }
        }

        public void SetTyingOperatorId(OperatorId tyingOperatorId)
        {
            Validator.ThrowIfNull(() => tyingOperatorId);
            if (tyingOperatorId != TyingOperatorId)
            {
                TyingOperatorId = tyingOperatorId;
                ReadModel.TyingOperatorId = TyingOperatorId.Value;

                MarkModified();
            }
        }

        public void SetLoomMachineId(MachineId loomMachineId)
        {
            Validator.ThrowIfNull(() => loomMachineId);
            if (loomMachineId != LoomMachineId)
            {
                LoomMachineId = loomMachineId;
                ReadModel.LoomMachineId = LoomMachineId.Value;

                MarkModified();
            }
        }

        public void SetLoomOperatorId(OperatorId loomOperatorId)
        {
            Validator.ThrowIfNull(() => loomOperatorId);
            if (loomOperatorId != LoomOperatorId)
            {
                LoomOperatorId = loomOperatorId;
                ReadModel.LoomOperatorId = LoomOperatorId.Value;

                MarkModified();
            }
        }

        public void SetCounterPerOperator(double counterPerOperator)
        {
            if (counterPerOperator != CounterPerOperator)
            {
                CounterPerOperator = counterPerOperator;
                ReadModel.CounterPerOperator = CounterPerOperator;

                MarkModified();
            }
        }

        public void SetDateTimeMachine(DateTimeOffset dateTimeMachine)
        {
            if (dateTimeMachine != DateTimeMachine)
            {
                DateTimeMachine = dateTimeMachine;
                ReadModel.DateTimeMachine = DateTimeMachine;

                MarkModified();
            }
        }

        public void SetShiftDocumentId(ShiftId newShiftDocumentId)
        {
            if (ShiftDocumentId != newShiftDocumentId)
            {
                ShiftDocumentId = newShiftDocumentId;
                ReadModel.ShiftDocumentId = newShiftDocumentId.Value;

                MarkModified();
            }
        }

        public void SetInformation(string information)
        {
            if (Information != information)
            {
                Information = information;
                ReadModel.Information = Information;

                MarkModified();

            }
        }

        public void SetMachineStatus(string newMachineStatus)
        {
            if (MachineStatus != newMachineStatus)
            {
                MachineStatus = newMachineStatus;
                ReadModel.MachineStatus = MachineStatus;

                MarkModified();
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override DailyOperationLoomHistory GetEntity()
        {
            return this;
        }
    }
}
