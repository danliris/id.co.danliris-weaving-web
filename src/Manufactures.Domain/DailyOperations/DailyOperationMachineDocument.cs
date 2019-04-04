using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Entities;
using Manufactures.Domain.DailyOperations.ReadModels;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.DailyOperations
{
    public class DailyOperationMachineDocument : AggregateRoot<DailyOperationMachineDocument, DailyOperationMachineDocumentReadModel>
    {
        public DateTimeOffset Time { get; private set; }
        public string Information { get; private set; }
        public MachineId MachineId { get; private set; }
        public UnitId UnitId { get; private set; }
        public IReadOnlyCollection<DailyOperationMachineDetail> DailyOperationMachineDetails { get; private set; }

        public DailyOperationMachineDocument(Guid id, DateTimeOffset time, string information, MachineId machineId, UnitId unitId) :base(id)
        {
            Identity = id;
            Time = time;
            Information = information;
            MachineId = machineId;
            UnitId = unitId;
            DailyOperationMachineDetails = new List<DailyOperationMachineDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationMachineDocumentReadModel(Identity)
            {
                Time = this.Time,
                Information = this.Information,
                MachineId = this.MachineId,
                UnitId = this.UnitId,
                DailyOperationMachineDetails = this.DailyOperationMachineDetails.ToList()
            };
        }

        public DailyOperationMachineDocument(DailyOperationMachineDocumentReadModel readModel) : base(readModel)
        {
            this.Time = readModel.Time;
            this.Information = readModel.Information;
            this.MachineId = readModel.MachineId;
            this.UnitId = readModel.UnitId;
            this.DailyOperationMachineDetails = readModel.DailyOperationMachineDetails;
        }

        public void AddDailyOperationMachine(DailyOperationMachineDetail dailyOperationMachineDetail)
        {
            var list = DailyOperationMachineDetails.ToList();
            list.Add(dailyOperationMachineDetail);
            DailyOperationMachineDetails = list;
            ReadModel.DailyOperationMachineDetails = DailyOperationMachineDetails.ToList();

            MarkModified();
        }

        protected override DailyOperationMachineDocument GetEntity()
        {
            return this;
        }
    }
}
