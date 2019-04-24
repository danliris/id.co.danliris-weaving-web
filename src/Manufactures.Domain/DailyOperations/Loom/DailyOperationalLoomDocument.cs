using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Loom
{
    public class DailyOperationalLoomDocument : AggregateRoot<DailyOperationalLoomDocument, DailyOperationalMachineLoomReadModel>
    {
        public DateTimeOffset DateOperated { get; private set; }
        public MachineId MachineId { get; private set; }
        public UnitId UnitId { get; private set; }
        public string Status { get; private set; }
        public IReadOnlyCollection<DailyOperationalLoomDetail> DailyOperationMachineDetails { get; private set; }

        public DailyOperationalLoomDocument(Guid id, MachineId machineId, UnitId unitId, string status) :base(id)
        {
            Identity = id;
            MachineId = machineId;
            UnitId = unitId;
            Status = status;
            DailyOperationMachineDetails = new List<DailyOperationalLoomDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationalMachineLoomReadModel(Identity)
            {
                MachineId = this.MachineId.Value,
                UnitId = this.UnitId.Value,
                Status = this.Status,
                DailyOperationMachineDetails = this.DailyOperationMachineDetails.ToList()
            };
        }

        public DailyOperationalLoomDocument(DailyOperationalMachineLoomReadModel readModel) : base(readModel)
        {
            this.DateOperated = readModel.CreatedDate;
            this.MachineId = readModel.MachineId.HasValue ? new MachineId(readModel.MachineId.Value) : null;
            this.UnitId = readModel.UnitId.HasValue ? new UnitId(readModel.UnitId.Value) : null;
            this.Status = readModel.Status; 
            this.DailyOperationMachineDetails = readModel.DailyOperationMachineDetails;
        }

        public void AddDailyOperationMachineDetail(DailyOperationalLoomDetail dailyOperationMachineDetail)
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

        public void SetStatus(string value)
        {
            Status = value;
            ReadModel.Status = Status;

            MarkModified();
        }

        protected override DailyOperationalLoomDocument GetEntity()
        {
            return this;
        }
    }
}
