using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomProduct : AggregateRoot<DailyOperationLoomProduct, DailyOperationLoomProductReadModel>
    {
        public double StartCounter { get; private set; }
        public double FinishCounter { get; private set; }
        public double MachineSpeed { get; private set; }
        public double SCMPX { get; private set; }
        public double Efficiency { get; private set; }
        public double F { get; private set; }
        public double W { get; private set; }
        public double L { get; private set; }
        public double T { get; private set; }
        public UomId UomDocumentId { get; private set; }
        public string UomUnit { get; private set; }
        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomProduct(Guid identity,
                                         double startCounter,
                                         Guid dailyOperationLoomDocumentId) : base(identity)
        {
            Identity = identity;
            StartCounter = startCounter;
            DailyOperationLoomDocumentId = dailyOperationLoomDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationLoomProductReadModel(Identity)
            {
                StartCounter = StartCounter,
                FinishCounter = FinishCounter,
                MachineSpeed = MachineSpeed,
                SCMPX = SCMPX,
                Efficiency = Efficiency,
                F = F,
                W = W,
                L = L,
                T = T,
                UomDocumentId = UomDocumentId.Value,
                UomUnit = UomUnit,
                DailyOperationLoomDocumentId = DailyOperationLoomDocumentId
            };
        }

        public DailyOperationLoomProduct(DailyOperationLoomProductReadModel readModel) : base(readModel)
        {
            StartCounter = readModel.StartCounter;
            FinishCounter = readModel.FinishCounter;
            MachineSpeed = readModel.MachineSpeed;
            SCMPX = readModel.SCMPX;
            Efficiency = readModel.Efficiency;
            F = readModel.F;
            W = readModel.W;
            L = readModel.L;
            T = readModel.T;
            UomDocumentId = new UomId(readModel.UomDocumentId);
            UomUnit = readModel.UomUnit;
            DailyOperationLoomDocumentId = readModel.DailyOperationLoomDocumentId;
        }

        public void SetStartCounter(double startCounter)
        {
            if (startCounter != StartCounter)
            {
                StartCounter = startCounter;
                ReadModel.StartCounter = StartCounter;

                MarkModified();
            }
        }

        public void SetFinishCounter(double finishCounter)
        {
            if (finishCounter != FinishCounter)
            {
                FinishCounter = finishCounter;
                ReadModel.FinishCounter = FinishCounter;

                MarkModified();
            }
        }

        public void SetMachineSpeed(double machineSpeed)
        {
            if (machineSpeed != MachineSpeed)
            {
                MachineSpeed = machineSpeed;
                ReadModel.MachineSpeed = MachineSpeed;

                MarkModified();
            }
        }

        public void SetSCMPX(double scmpx)
        {
            if (scmpx != SCMPX)
            {
                SCMPX = scmpx;
                ReadModel.SCMPX = SCMPX;

                MarkModified();
            }
        }

        public void SetEfficiency(double efficiency)
        {
            if (efficiency != Efficiency)
            {
                Efficiency = efficiency;
                ReadModel.Efficiency = Efficiency;

                MarkModified();
            }
        }

        public void SetF(double f)
        {
            if (f != F)
            {
                F = f;
                ReadModel.F = F;

                MarkModified();
            }
        }

        public void SetW(double w)
        {
            if (w != W)
            {
                W = w;
                ReadModel.W = W;

                MarkModified();
            }
        }

        public void SetL(double l)
        {
            if (l != L)
            {
                L = l;
                ReadModel.L = L;

                MarkModified();
            }
        }

        public void SetT(double t)
        {
            if (t != T)
            {
                T = t;
                ReadModel.T = T;

                MarkModified();
            }
        }

        public void SetUomDocumentId(UomId uomDocumentId)
        {
            Validator.ThrowIfNull(() => uomDocumentId);
            if (uomDocumentId != UomDocumentId)
            {
                UomDocumentId = uomDocumentId;
                ReadModel.UomDocumentId = UomDocumentId.Value;

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

        protected override DailyOperationLoomProduct GetEntity()
        {
            throw new NotImplementedException();
        }
    }
}
