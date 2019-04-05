using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Entities;
using Manufactures.Domain.DailyOperations.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations
{
    public class DailyOperationMachineDocument : AggregateRoot<DailyOperationMachineDocument, DailyOperationMachineDocumentReadModel>
    {
        public DateTimeOffset DateOperated { get; private set; }
        public MachineId MachineId { get; private set; }
        public UnitId UnitId { get; private set; }
        public IReadOnlyCollection<DailyOperationMachineDetail> DailyOperationMachineDetails { get; private set; }

        public DailyOperationMachineDocument(Guid id, MachineId machineId, UnitId unitId) :base(id)
        {
            Identity = id;
            MachineId = machineId;
            UnitId = unitId;
            DailyOperationMachineDetails = new List<DailyOperationMachineDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationMachineDocumentReadModel(Identity)
            {
                MachineId = this.MachineId.Value,
                UnitId = this.UnitId.Value,
                DailyOperationMachineDetails = this.DailyOperationMachineDetails.ToList()
            };
        }

        public DailyOperationMachineDocument(DailyOperationMachineDocumentReadModel readModel) : base(readModel)
        {
            this.DateOperated = readModel.CreatedDate;
            this.MachineId = readModel.MachineId.HasValue ? new MachineId(readModel.MachineId.Value) : null;
            this.UnitId = readModel.UnitId.HasValue ? new UnitId(readModel.UnitId.Value) : null;
            this.DailyOperationMachineDetails = readModel.DailyOperationMachineDetails;
        }

        public void AddDailyOperationMachineDetail(DailyOperationMachineDetail dailyOperationMachineDetail)
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

        protected override DailyOperationMachineDocument GetEntity()
        {
            return this;
        }
    }
}
