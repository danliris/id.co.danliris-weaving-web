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
        public Guid BeamDocumentId { get; private set; }

        public string BeamNumber { get; private set; }

        public Guid? TyingMachineId { get; private set; }

        public Guid? TyingOperatorId { get; private set; }

        public Guid LoomMachineId { get; private set; }

        public Guid LoomOperatorId { get; private set; }

        public double? CounterPerOperator { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public Guid ShiftDocumentId { get; private set; }

        public string Information { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomHistory(Guid identity,
                                         Guid beamDocumentId,
                                         string beamNumber,
                                         Guid loomMachineId,
                                         Guid loomOperatorId,
                                         double counterPerOperator,
                                         DateTimeOffset dateTimeMachine,
                                         Guid shiftDocumentId,
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
                BeamDocumentId = BeamDocumentId,
                BeamNumber = BeamNumber,
                TyingMachineId = TyingMachineId ?? Guid.Empty,
                TyingOperatorId = TyingOperatorId ?? Guid.Empty,
                LoomMachineId = LoomMachineId,
                LoomOperatorId = LoomOperatorId,
                CounterPerOperator = CounterPerOperator ?? 0,
                DateTimeMachine = DateTimeMachine,
                ShiftDocumentId = ShiftDocumentId,
                MachineStatus = MachineStatus,
                DailyOperationLoomDocumentId = DailyOperationLoomDocumentId
            };

            //ReadModel.AddDomainEvent(new OnAddDailyOperationLoomBeamHistory(Identity));
        }

        public DailyOperationLoomHistory(DailyOperationLoomHistoryReadModel readModel) : base(readModel)
        {
            BeamDocumentId = readModel.BeamDocumentId;
            BeamNumber = readModel.BeamNumber;
            TyingMachineId = readModel.TyingMachineId;
            TyingOperatorId = readModel.TyingOperatorId;
            LoomMachineId = readModel.LoomMachineId;
            LoomOperatorId = readModel.LoomOperatorId;
            CounterPerOperator = readModel.CounterPerOperator;
            DateTimeMachine = readModel.DateTimeMachine;
            ShiftDocumentId = readModel.ShiftDocumentId;
            Information = readModel.Information;
            MachineStatus = readModel.MachineStatus;
            DailyOperationLoomDocumentId = readModel.DailyOperationLoomDocumentId;
        }

        public void SetBeamDocumentId(Guid beamDocumentId)
        {
            if (BeamDocumentId != beamDocumentId)
            {
                BeamDocumentId = beamDocumentId;
                ReadModel.BeamDocumentId = BeamDocumentId;
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

        public void SetTyingMachineId(Guid tyingMachineId)
        {
            if (tyingMachineId != TyingMachineId)
            {
                TyingMachineId = tyingMachineId;
                ReadModel.TyingMachineId = TyingMachineId ?? Guid.Empty;

                MarkModified();
            }
        }

        public void SetTyingOperatorId(Guid tyingOperatorId)
        {
            if (tyingOperatorId != TyingOperatorId)
            {
                TyingOperatorId = tyingOperatorId;
                ReadModel.TyingOperatorId = TyingOperatorId ?? Guid.Empty;

                MarkModified();
            }
        }

        public void SetLoomMachineId(Guid loomMachineId)
        {
            if (loomMachineId != LoomMachineId)
            {
                LoomMachineId = loomMachineId;
                ReadModel.LoomMachineId = LoomMachineId;

                MarkModified();
            }
        }

        public void SetLoomOperatorId(Guid loomOperatorId)
        {
            if (loomOperatorId != LoomOperatorId)
            {
                LoomOperatorId = loomOperatorId;
                ReadModel.LoomOperatorId = LoomOperatorId;

                MarkModified();
            }
        }

        public void SetCounterPerOperator(double counterPerOperator)
        {
            if (counterPerOperator != CounterPerOperator)
            {
                CounterPerOperator = counterPerOperator;
                ReadModel.CounterPerOperator = CounterPerOperator ?? 0;

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

        public void SetShiftDocumentId(Guid newShiftDocumentId)
        {
            if (ShiftDocumentId != newShiftDocumentId)
            {
                ShiftDocumentId = newShiftDocumentId;
                ReadModel.ShiftDocumentId = newShiftDocumentId;

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
