using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Entities;
using Manufactures.Domain.DailyOperations.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations
{
    public class DailyOperationalMachineDocument : AggregateRoot<DailyOperationalMachineDocument, DailyOperationalMachineDocumentReadModel>
    {
        public DateTimeOffset DateOperated { get; private set; }
        public MachineId MachineId { get; private set; }
        public UnitId UnitId { get; private set; }
        public IReadOnlyCollection<DailyOperationalMachineDetail> DailyOperationMachineDetails { get; private set; }

        public DailyOperationalMachineDocument(Guid id, MachineId machineId, UnitId unitId) :base(id)
        {
            Identity = id;
            MachineId = machineId;
            UnitId = unitId;
            DailyOperationMachineDetails = new List<DailyOperationalMachineDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationalMachineDocumentReadModel(Identity)
            {
                MachineId = this.MachineId.Value,
                UnitId = this.UnitId.Value,
                DailyOperationMachineDetails = this.DailyOperationMachineDetails.ToList()
            };
        }

        public DailyOperationalMachineDocument(DailyOperationalMachineDocumentReadModel readModel) : base(readModel)
        {
            this.DateOperated = readModel.CreatedDate;
            this.MachineId = readModel.MachineId.HasValue ? new MachineId(readModel.MachineId.Value) : null;
            this.UnitId = readModel.UnitId.HasValue ? new UnitId(readModel.UnitId.Value) : null;
            this.DailyOperationMachineDetails = readModel.DailyOperationMachineDetails;
        }

        public void AddDailyOperationMachineDetail(DailyOperationalMachineDetail dailyOperationMachineDetail)
        {
            var list = DailyOperationMachineDetails.ToList();
            list.Add(dailyOperationMachineDetail);
            DailyOperationMachineDetails = list;
            ReadModel.DailyOperationMachineDetails = DailyOperationMachineDetails.ToList();

            MarkModified();
        }

        public void RemoveDailyOperationMachineDetail(Guid identity)
        {
            var detail = DailyOperationMachineDetails.Where(o => o.Identity == identity).FirstOrDefault();
            var list = DailyOperationMachineDetails.ToList();

            list.Remove(detail);
            DailyOperationMachineDetails = list;
            ReadModel.DailyOperationMachineDetails = DailyOperationMachineDetails.ToList();

            MarkModified();
        }

        public void SetUnit(UnitId value)
        {
            if (value.Value != UnitId.Value)
            {
                UnitId = value;
                ReadModel.UnitId = UnitId.Value;

                MarkModified();
            }
        }

        public void SetMachine(MachineId value)
        {
            if(value.Value != MachineId.Value)
            {
                MachineId = value;
                ReadModel.MachineId = MachineId.Value;

                MarkModified();
            }
        }

        protected override DailyOperationalMachineDocument GetEntity()
        {
            return this;
        }
    }
}
