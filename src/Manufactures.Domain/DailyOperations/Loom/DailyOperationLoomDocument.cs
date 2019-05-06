using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Loom
{
    public class DailyOperationLoomDocument : AggregateRoot<DailyOperationLoomDocument, DailyOperationLoomReadModel>
    {
        public DateTimeOffset DateOperated { get; private set; }
        public MachineId MachineId { get; private set; }
        public UnitId UnitId { get; private set; }
        public string DailyOperationStatus { get; private set; }
        public DailyOperationId DailyOperationSizingId { get; private set; }
        public IReadOnlyCollection<DailyOperationLoomDetail> DailyOperationMachineDetails { get; private set; }

        public DailyOperationLoomDocument(Guid id,
                                          MachineId machineId,
                                          UnitId unitId,
                                          string dailyOperationStatus) : base(id)
        {
            Identity = id;
            MachineId = machineId;
            UnitId = unitId;
            DailyOperationStatus = dailyOperationStatus;
            DailyOperationMachineDetails = new List<DailyOperationLoomDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationLoomReadModel(Identity)
            {
                MachineId = this.MachineId.Value,
                UnitId = this.UnitId.Value,
                DailyOperationStatus = this.DailyOperationStatus,
                DailyOperationLoomDetails = this.DailyOperationMachineDetails.ToList()
            };

            ReadModel.AddDomainEvent(new OnAddDailyOperationLoom(Identity));
        }

        public DailyOperationLoomDocument(DailyOperationLoomReadModel readModel) : base(readModel)
        {
            this.DateOperated = readModel.CreatedDate;
            this.MachineId = readModel.MachineId.HasValue ? new MachineId(readModel.MachineId.Value) : null;
            this.UnitId = readModel.UnitId.HasValue ? new UnitId(readModel.UnitId.Value) : null;
            this.DailyOperationStatus = readModel.DailyOperationStatus;
            this.DailyOperationMachineDetails = readModel.DailyOperationLoomDetails;
        }

        public void AddDailyOperationMachineDetail(DailyOperationLoomDetail dailyOperationMachineDetail)
        {
            var list = DailyOperationMachineDetails.ToList();
            list.Add(dailyOperationMachineDetail);
            DailyOperationMachineDetails = list;
            ReadModel.DailyOperationLoomDetails = DailyOperationMachineDetails.ToList();

            MarkModified();
        }

        public void RemoveDailyOperationMachineDetail(Guid identity)
        {
            var detail = DailyOperationMachineDetails.Where(o => o.Identity == identity).FirstOrDefault();
            var list = DailyOperationMachineDetails.ToList();

            list.Remove(detail);
            DailyOperationMachineDetails = list;
            ReadModel.DailyOperationLoomDetails = DailyOperationMachineDetails.ToList();

            MarkModified();
        }

        public void SetDailyOperationStatus(string value)
        {
            if(DailyOperationStatus != value)
            {
                DailyOperationStatus = value;
                ReadModel.DailyOperationStatus = DailyOperationStatus;

                MarkModified();
            }
        }

        protected override DailyOperationLoomDocument GetEntity()
        {
            return this;
        }
    }
}
