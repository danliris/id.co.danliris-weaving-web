using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomBeamProduct : AggregateRoot<DailyOperationLoomBeamProduct, DailyOperationLoomBeamProductReadModel>
    {
        public string BeamOrigin { get; private set; }

        public BeamId BeamDocumentId { get; private set; }

        public double CombNumber { get; private set; }

        public MachineId MachineDocumentId { get; private set; }

        public DateTimeOffset LatestDateTimeBeamProduct { get; private set; }

        public string LoomProcess { get; private set; }

        public string BeamProductStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomBeamProduct(Guid identity, string beamOrigin, BeamId beamDocumentId, double combNumber, MachineId machineDocumentId, DateTimeOffset latestDateTimeBeamProduct, string loomProcess, string beamProductStatus, Guid dailyOperationLoomDocumentId) : base(identity)
        {
            MarkTransient();
            BeamOrigin = beamOrigin;
            BeamDocumentId = beamDocumentId;
            CombNumber = combNumber;
            MachineDocumentId = machineDocumentId;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            LoomProcess = loomProcess;
            BeamProductStatus = beamProductStatus;
            DailyOperationLoomDocumentId = dailyOperationLoomDocumentId;

            ReadModel = new DailyOperationLoomBeamProductReadModel(Identity)
            {
                BeamOrigin = BeamOrigin,
                BeamDocumentId = BeamDocumentId.Value,
                CombNumber = CombNumber,
                MachineDocumentId = MachineDocumentId.Value,
                LatestDateTimeBeamProduct = LatestDateTimeBeamProduct,
                LoomProcess = LoomProcess,
                BeamProductStatus = BeamProductStatus,
                DailyOperationLoomDocumentId = DailyOperationLoomDocumentId
            };

            ReadModel.AddDomainEvent(new OnAddDailyOperationLoomBeamProduct(Identity));
        }

        public DailyOperationLoomBeamProduct(DailyOperationLoomBeamProductReadModel readModel) : base(readModel)
        {
            BeamOrigin = readModel.BeamOrigin;
            BeamDocumentId = new BeamId(readModel.BeamDocumentId);
            CombNumber = readModel.CombNumber;
            MachineDocumentId = new MachineId(readModel.MachineDocumentId);
            LatestDateTimeBeamProduct = readModel.LatestDateTimeBeamProduct;
            LoomProcess = readModel.LoomProcess;
            BeamProductStatus = readModel.BeamProductStatus;
            DailyOperationLoomDocumentId = readModel.DailyOperationLoomDocumentId;
        }

        protected override DailyOperationLoomBeamProduct GetEntity()
        {
            return this;
        }

        public void SetBeamOrigin(string newBeamOrigin)
        {
            if(BeamOrigin != newBeamOrigin)
            {
                BeamOrigin = newBeamOrigin;
                ReadModel.BeamOrigin = BeamOrigin;
                MarkModified();
            }
            
        }

        public void SetBeamDocumentId(BeamId newBeamDocumentId)
        {
            if (BeamDocumentId != newBeamDocumentId)
            {
                BeamDocumentId = newBeamDocumentId;
                ReadModel.BeamDocumentId = BeamDocumentId.Value;
                MarkModified();
            }

        }

        public void SetCombNumber(double newCombNumber)
        {
            if (CombNumber != newCombNumber)
            {
                CombNumber = newCombNumber;
                ReadModel.CombNumber = CombNumber;
                MarkModified();
            }

        }

        public void SetMachineDocumentId(MachineId newMachineDocumentId)
        {
            if (MachineDocumentId != newMachineDocumentId)
            {
                MachineDocumentId = newMachineDocumentId;
                ReadModel.MachineDocumentId = MachineDocumentId.Value;
                MarkModified();
            }
        }

        public void SetLatestDateTimeBeamProduct(DateTimeOffset newLatestDateTimeBeamProduct)
        {
            if (LatestDateTimeBeamProduct != newLatestDateTimeBeamProduct)
            {
                LatestDateTimeBeamProduct = newLatestDateTimeBeamProduct;
                ReadModel.LatestDateTimeBeamProduct = LatestDateTimeBeamProduct;
                MarkModified();
            }
        }

        public void SetLoomProcess(string newLoomProcess)
        {
            if (LoomProcess != newLoomProcess)
            {
                LoomProcess = newLoomProcess;
                ReadModel.LoomProcess = LoomProcess;
                MarkModified();
            }
        }

        public void SetBeamProductStatus(string newBeamProductStatus)
        {
            if (BeamProductStatus != newBeamProductStatus)
            {
                BeamProductStatus = newBeamProductStatus;
                ReadModel.BeamProductStatus = BeamProductStatus;
                MarkModified();
            }
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }
    }
}
