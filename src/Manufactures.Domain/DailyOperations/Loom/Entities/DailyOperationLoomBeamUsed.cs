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

        public Guid BeamDocumentId { get; private set; }

        public string BeamNumber { get; private set; }

        public double? StartCounter { get; private set; }

        public double? FinishCounter { get; private set; }

        public double? MachineSpeed { get; private set; }

        public double? SCMPX { get; private set; }

        public double? Efficiency { get; private set; }

        public double? F { get; private set; }

        public double? W { get; private set; }

        public double? L { get; private set; }

        public double? T { get; private set; }

        public int UomDocumentId { get; private set; }

        public string UomUnit { get; private set; }

        public DateTimeOffset LastDateTimeProcessed { get; private set; }

        public string BeamUsedStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomBeamUsed(Guid identity, 
                                          string beamOrigin, 
                                          Guid beamDocumentId, 
                                          string beamNumber, 
                                          DateTimeOffset lastDateTimeProcessed,
                                          string beamUsedStatus,
                                          Guid dailyOperationLoomDocumentId) : base(identity)
        {
            Identity = identity;
            BeamOrigin = beamOrigin;
            BeamDocumentId = beamDocumentId;
            BeamNumber = beamNumber;
            LastDateTimeProcessed = lastDateTimeProcessed;
            BeamUsedStatus = beamUsedStatus;
            DailyOperationLoomDocumentId = dailyOperationLoomDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationLoomBeamUsedReadModel(Identity)
            {
                BeamOrigin = BeamOrigin,
                BeamDocumentId = BeamDocumentId,
                BeamNumber = BeamNumber,
                StartCounter = StartCounter ?? 0,
                FinishCounter = FinishCounter ?? 0,
                MachineSpeed = MachineSpeed ?? 0,
                SCMPX = SCMPX ?? 0,
                Efficiency = Efficiency ?? 0,
                F=F ?? 0,
                W=W ?? 0,
                L=L ?? 0,
                T=T ?? 0,
                UomDocumentId = UomDocumentId,
                UomUnit = UomUnit,
                LastDateTimeProcessed = LastDateTimeProcessed,
                BeamUsedStatus = BeamUsedStatus,
                DailyOperationLoomDocumentId = DailyOperationLoomDocumentId
            };

            //ReadModel.AddDomainEvent(new OnAddDailyOperationLoomBeamProduct(Identity));
        }

        public DailyOperationLoomBeamUsed(DailyOperationLoomBeamUsedReadModel readModel) : base(readModel)
        {
            BeamOrigin = readModel.BeamOrigin;
            BeamDocumentId = readModel.BeamDocumentId;
            BeamNumber = readModel.BeamNumber;
            StartCounter = readModel.StartCounter;
            FinishCounter = readModel.FinishCounter;
            MachineSpeed = readModel.MachineSpeed;
            SCMPX = readModel.SCMPX;
            Efficiency = readModel.Efficiency;
            F = readModel.F;
            W = readModel.W;
            L = readModel.L;
            T = readModel.T;
            UomDocumentId = readModel.UomDocumentId;
            UomUnit = readModel.UomUnit;
            LastDateTimeProcessed = readModel.LastDateTimeProcessed;
            BeamUsedStatus = readModel.BeamUsedStatus;
            DailyOperationLoomDocumentId = readModel.DailyOperationLoomDocumentId;
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

        public void SetBeamDocumentId(Guid beamDocumentId)
        {
            Validator.ThrowIfNull(() => beamDocumentId);
            if (BeamDocumentId != beamDocumentId)
            {
                BeamDocumentId = beamDocumentId;
                ReadModel.BeamDocumentId = BeamDocumentId;
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

        public void SetStartCounter(double startCounter)
        {
            if (startCounter != StartCounter)
            {
                StartCounter = startCounter;
                ReadModel.StartCounter = StartCounter ?? 0;

                MarkModified();
            }
        }

        public void SetFinishCounter(double finishCounter)
        {
            if (finishCounter != FinishCounter)
            {
                FinishCounter = finishCounter;
                ReadModel.FinishCounter = FinishCounter ?? 0;

                MarkModified();
            }
        }

        public void SetMachineSpeed(double machineSpeed)
        {
            if (machineSpeed != MachineSpeed)
            {
                MachineSpeed = machineSpeed;
                ReadModel.MachineSpeed = MachineSpeed ?? 0;

                MarkModified();
            }
        }

        public void SetSCMPX(double scmpx)
        {
            if (scmpx != SCMPX)
            {
                SCMPX = scmpx;
                ReadModel.SCMPX = SCMPX ?? 0;

                MarkModified();
            }
        }

        public void SetEfficiency(double efficiency)
        {
            if (efficiency != Efficiency)
            {
                Efficiency = efficiency;
                ReadModel.Efficiency = Efficiency ?? 0;

                MarkModified();
            }
        }

        public void SetF(double f)
        {
            if (f != F)
            {
                F = f;
                ReadModel.F = F ?? 0;

                MarkModified();
            }
        }

        public void SetW(double w)
        {
            if (w != W)
            {
                W = w;
                ReadModel.W = W ?? 0;

                MarkModified();
            }
        }

        public void SetL(double l)
        {
            if (l != L)
            {
                L = l;
                ReadModel.L = L ?? 0;

                MarkModified();
            }
        }

        public void SetT(double t)
        {
            if (t != T)
            {
                T = t;
                ReadModel.T = T ?? 0;

                MarkModified();
            }
        }

        public void SetUomDocumentId(int uomDocumentId)
        {
            //Validator.ThrowIfNull(() => uomDocumentId);
            if (uomDocumentId != UomDocumentId)
            {
                UomDocumentId = uomDocumentId;
                ReadModel.UomDocumentId = UomDocumentId;

                MarkModified();
            }
        }

        public void SetUomUnit(string uomUnit)
        {
            Validator.ThrowIfNull(() => uomUnit);
            if (uomUnit != UomUnit)
            {
                UomUnit = uomUnit;
                ReadModel.UomUnit = UomUnit;

                MarkModified();
            }
        }

        public void SetLastDateTimeProcessed(DateTimeOffset lastDateTimeProcessed)
        {
            if (lastDateTimeProcessed != LastDateTimeProcessed)
            {
                LastDateTimeProcessed = lastDateTimeProcessed;
                ReadModel.LastDateTimeProcessed = LastDateTimeProcessed;

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

        protected override DailyOperationLoomBeamUsed GetEntity()
        {
            return this;
        }
    }
}
