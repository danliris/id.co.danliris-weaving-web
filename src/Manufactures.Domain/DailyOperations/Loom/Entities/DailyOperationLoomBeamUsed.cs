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
    public class DailyOperationLoomBeamUsed : AggregateRoot<DailyOperationLoomBeamUsed, DailyOperationLoomBeamUsedReadModel>
    {
        public string BeamOrigin { get; private set; }
        public BeamId BeamDocumentId { get; private set; }
        public string BeamNumber { get; private set; }
        public MachineId TyingMachineId { get; private set; }
        public OperatorId TyingOperatorId { get; private set; }
        public MachineId LoomMachineId { get; private set; }
        public OperatorId LoomOperatorId { get; private set; }
        public DateTimeOffset DateTimeProcessed { get; private set; }
        public ShiftId ShiftDocumentId { get; private set; }
        public string Process { get; private set; }
        public string BeamUsedStatus { get; private set; }
        public Guid DailyOperationLoomDocumentId { get; set; }
        public Guid DailyOperationLoomProductId { get; set; }

        public DailyOperationLoomBeamUsed(Guid identity, 
                                          string beamOrigin, 
                                          BeamId beamDocumentId, 
                                          string beamNumber, 
                                          //MachineId tyingMachineId,
                                          //OperatorId tyingOperatorId,
                                          MachineId loomMachineId,
                                          OperatorId loomOperatorId,
                                          DateTimeOffset dateTimeProcessed,
                                          ShiftId shiftDocumentId,
                                          string process, 
                                          string beamUsedStatus,
                                          //Guid dailyOperationLoomProductId, 
                                          Guid dailyOperationLoomDocumentId) : base(identity)
        {
            Identity = identity;
            BeamOrigin = beamOrigin;
            BeamDocumentId = beamDocumentId;
            BeamNumber = beamNumber;
            //TyingMachineId = tyingMachineId;
            //TyingOperatorId = tyingOperatorId;
            LoomMachineId = loomMachineId;
            LoomOperatorId = loomOperatorId;
            DateTimeProcessed = dateTimeProcessed;
            ShiftDocumentId = shiftDocumentId;
            Process = process;
            BeamUsedStatus = beamUsedStatus;
            //DailyOperationLoomProductId = dailyOperationLoomProductId;
            DailyOperationLoomDocumentId = dailyOperationLoomDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationLoomBeamUsedReadModel(Identity)
            {
                BeamOrigin = BeamOrigin,
                BeamDocumentId = BeamDocumentId.Value,
                BeamNumber = BeamNumber,
                TyingMachineId = TyingMachineId.Value,
                TyingOperatorId = TyingOperatorId.Value,
                LoomMachineId = LoomMachineId.Value,
                LoomOperatorId = LoomOperatorId.Value,
                DateTimeProcessed = DateTimeProcessed,
                ShiftDocumentId = ShiftDocumentId.Value,
                Process = Process,
                BeamUsedStatus = BeamUsedStatus,
                DailyOperationLoomProductId = DailyOperationLoomProductId,
                DailyOperationLoomDocumentId = DailyOperationLoomDocumentId
            };

            //ReadModel.AddDomainEvent(new OnAddDailyOperationLoomBeamProduct(Identity));
        }

        public DailyOperationLoomBeamUsed(DailyOperationLoomBeamUsedReadModel readModel) : base(readModel)
        {
            BeamOrigin = readModel.BeamOrigin;
            BeamDocumentId = new BeamId(readModel.BeamDocumentId);
            BeamNumber = readModel.BeamNumber;
            TyingMachineId = new MachineId(readModel.TyingMachineId);
            TyingOperatorId = new OperatorId(readModel.TyingOperatorId);
            LoomMachineId = new MachineId(readModel.LoomMachineId);
            LoomOperatorId = new OperatorId(readModel.LoomOperatorId);
            DateTimeProcessed = readModel.DateTimeProcessed;
            ShiftDocumentId = new ShiftId(readModel.ShiftDocumentId);
            Process = readModel.Process;
            BeamUsedStatus = readModel.BeamUsedStatus;
            DailyOperationLoomProductId = readModel.DailyOperationLoomProductId;
            DailyOperationLoomDocumentId = readModel.DailyOperationLoomDocumentId;
        }

        protected override DailyOperationLoomBeamUsed GetEntity()
        {
            return this;
        }

        public void SetBeamOrigin(string beamOrigin)
        {
            Validator.ThrowIfNull(() => beamOrigin);
            if (BeamOrigin != beamOrigin)
            {
                BeamOrigin = beamOrigin;
                ReadModel.BeamOrigin = BeamOrigin;
                MarkModified();
            }            
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

        public void SetBeamNumber(string beamNumber)
        {
            Validator.ThrowIfNull(() => beamNumber);
            if (beamNumber != BeamNumber)
            {
                BeamNumber = beamNumber;
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
                ReadModel.TyingMachineId = LoomOperatorId.Value;

                MarkModified();
            }
        }

        public void SetDateTimeProcessed(DateTimeOffset dateTimeProcessed)
        {
            if (dateTimeProcessed != DateTimeProcessed)
            {
                DateTimeProcessed = dateTimeProcessed;
                ReadModel.DateTimeProcessed = DateTimeProcessed;

                MarkModified();
            }
        }

        public void SetShiftDocumentId(ShiftId shiftDocumentId)
        {
            Validator.ThrowIfNull(() => shiftDocumentId);
            if (shiftDocumentId != ShiftDocumentId)
            {
                ShiftDocumentId = shiftDocumentId;
                ReadModel.ShiftDocumentId = ShiftDocumentId.Value;

                MarkModified();
            }
        }

        public void SetProcess(string process)
        {
            Validator.ThrowIfNull(() => process);
            if (process != Process)
            {
                Process = process;
                ReadModel.Process = Process;

                MarkModified();
            }
        }

        public void SetBeamUsedStatus(string beamUsedStatus)
        {
            Validator.ThrowIfNull(() => beamUsedStatus);
            if (beamUsedStatus != BeamUsedStatus)
            {
                BeamUsedStatus = beamUsedStatus;
                ReadModel.BeamUsedStatus = BeamUsedStatus;

                MarkModified();
            }
        }

        public void SetLoomProductId(Guid loomProductId)
        {
            Validator.ThrowIfNull(() => loomProductId);
            if (loomProductId != DailyOperationLoomProductId)
            {
                DailyOperationLoomProductId = loomProductId;
                ReadModel.DailyOperationLoomProductId = DailyOperationLoomProductId;

                MarkModified();
            }
        }

        public void SetLoomDocumentId(Guid loomDocumentId)
        {
            Validator.ThrowIfNull(() => loomDocumentId);
            if (loomDocumentId != DailyOperationLoomDocumentId)
            {
                DailyOperationLoomDocumentId = loomDocumentId;
                ReadModel.DailyOperationLoomDocumentId = DailyOperationLoomDocumentId;

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
    }
}
