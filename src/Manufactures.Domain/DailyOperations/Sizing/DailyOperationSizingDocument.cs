using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Sizing
{
    public class DailyOperationSizingDocument : AggregateRoot<DailyOperationSizingDocument, DailyOperationSizingReadModel>
    {
        public DateTimeOffset ProductionDate { get; private set; }
        public MachineId MachineDocumentId { get; private set; }
        public UnitId WeavingUnitId { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingDetail> DailyOperationSizingDetails { get; private set; }

        public DailyOperationSizingDocument(Guid id, MachineId machineDocumentId, UnitId weavingUnitId):base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            WeavingUnitId = weavingUnitId;
            DailyOperationSizingDetails = new List<DailyOperationSizingDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationSizingReadModel(Identity)
            {
                MachineDocumentId = this.MachineDocumentId.Value,
                WeavingUnitId = this.WeavingUnitId.Value,
                DailyOperationSizingDetails = this.DailyOperationSizingDetails.ToList()
            };
        }
        public DailyOperationSizingDocument(DailyOperationSizingReadModel readModel) : base(readModel)
        {
            this.ProductionDate = readModel.CreatedDate;
            this.MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            this.WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            this.DailyOperationSizingDetails = readModel.DailyOperationSizingDetails;
        }

        public void AddDailyOperationSizingDetail(DailyOperationSizingDetail dailyOperationSizingDetail)
        {
            var list = DailyOperationSizingDetails.ToList();
            list.Add(dailyOperationSizingDetail);
            DailyOperationSizingDetails = list;
            ReadModel.DailyOperationSizingDetails = DailyOperationSizingDetails.ToList();

            MarkModified();
        }

        public void RemoveDailyOperationMachineDetail(Guid identity)
        {
            var detail = DailyOperationSizingDetails.Where(o => o.Identity == identity).FirstOrDefault();
            var list = DailyOperationSizingDetails.ToList();

            list.Remove(detail);
            DailyOperationSizingDetails = list;
            ReadModel.DailyOperationSizingDetails = DailyOperationSizingDetails.ToList();

            MarkModified();
        }

        protected override DailyOperationSizingDocument GetEntity()
        {
            return this;
        }
    }
}
