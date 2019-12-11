using Infrastructure.Domain;
using Infrastructure.Domain.Events;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomBeamProduct : EntityBase<DailyOperationLoomBeamProduct>
    {
        public string BeamOrigin { get; private set; }

        public Guid BeamDocumentId { get; private set; }

        public double CombNumber { get; private set; }

        public Guid MachineDocumentId { get; private set; }

        public DateTimeOffset LatestDateTimeBeamProduct { get; private set; }

        public string LoomProcess { get; private set; }

        public string BeamProductStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomReadModel DailyOperationLoomDocument { get; set; }

        public DailyOperationLoomBeamProduct(Guid identity) : base(identity)
        {
        }

        public DailyOperationLoomBeamProduct(Guid identity,
                                             string beamOrigin,
                                             BeamId beamDocumentId,
                                             double combNumber,
                                             MachineId machineDocumentId,
                                             DateTimeOffset latestDateTimeBeamProduct,
                                             string loomProcess,
                                             string beamProductStatus) : base(identity)
        {
            BeamOrigin = beamOrigin;
            BeamDocumentId = beamDocumentId.Value;
            CombNumber = combNumber;
            MachineDocumentId = machineDocumentId.Value;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            LoomProcess = loomProcess;
            BeamProductStatus = beamProductStatus;
        }

        public void SetBeamOrigin(string beamOrigin)
        {
            BeamOrigin = beamOrigin;
            MarkModified();
        }

        public void SetBeamDocumentId(BeamId beamDocumentId)
        {
            BeamDocumentId = beamDocumentId.Value;
            MarkModified();
        }

        public void SetCombNumber(double combNumber)
        {
            CombNumber = combNumber;
            MarkModified();
        }

        public void SetMachineDocumentId(MachineId machineDocumentId)
        {
            MachineDocumentId = machineDocumentId.Value;
            MarkModified();
        }

        public void SetLatestDateTimeBeamProduct(DateTimeOffset latestDateTimeBeamProduct)
        {
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            MarkModified();
        }

        public void SetLoomProcess(string loomProcess)
        {
            LoomProcess = loomProcess;
            MarkModified();
        }

        public void SetBeamProductStatus(string beamProductStatus)
        {
            BeamProductStatus = beamProductStatus;
            MarkModified();
        }

        protected override DailyOperationLoomBeamProduct GetEntity()
        {
            return this;
        }

        protected override void MarkRemoved()
        {
            DeletedBy = "System";
            Deleted = true;
            DeletedDate = DateTimeOffset.UtcNow;

            if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityDeleted<DailyOperationLoomBeamProduct>))
                this.AddDomainEvent(new OnEntityDeleted<DailyOperationLoomBeamProduct>(GetEntity()));

            // clear updated events
            if (this.DomainEvents.Any(o => o is OnEntityUpdated<DailyOperationLoomBeamProduct>))
            {
                this.DomainEvents.Where(o => o is OnEntityUpdated<DailyOperationLoomBeamProduct>)
                    .ToList()
                    .ForEach(o => this.RemoveDomainEvent(o));
            }
        }
    }
}
