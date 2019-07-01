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
    public class DailyOperationLoomDocument 
        : AggregateRoot<DailyOperationLoomDocument, DailyOperationLoomReadModel>
    {
        public UnitId UnitId { get; private set; }
        public MachineId MachineId { get; private set; }
        public BeamId BeamId { get; private set; }
        public OrderId OrderId { get; private set; }
        public string DailyOperationStatus { get; private set; }
        public DailyOperationMonitoringId DailyOperationMonitoringId { get; private set; }
        public IReadOnlyCollection<DailyOperationLoomDetail> DailyOperationMachineDetails { get; private set; }
        public double UsedYarn { get; private set; }

        public DailyOperationLoomDocument(Guid id,
                                          UnitId unitId,
                                          MachineId machineId,
                                          BeamId beamId,
                                          OrderId orderId,
                                          DailyOperationMonitoringId dailyOperationMonitoringId,
                                          string dailyOperationStatus) : base(id)
        {
            Identity = id;
            UnitId = unitId;
            MachineId = machineId;
            BeamId = beamId;
            OrderId = orderId;
            DailyOperationStatus = dailyOperationStatus;
            DailyOperationMonitoringId = dailyOperationMonitoringId;
            DailyOperationMachineDetails = new List<DailyOperationLoomDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationLoomReadModel(Identity)
            {
                UnitId = this.UnitId.Value,
                MachineId = this.MachineId.Value,
                BeamId = this.BeamId.Value,
                OrderId = this.OrderId.Value,
                DailyOperationStatus = this.DailyOperationStatus,
                DailyOperationMonitoringId = this.DailyOperationMonitoringId.Value,
                DailyOperationLoomDetails = this.DailyOperationMachineDetails.ToList()
            };

            ReadModel.AddDomainEvent(new OnAddDailyOperationLoom(Identity));
        }

        public DailyOperationLoomDocument(DailyOperationLoomReadModel readModel)
            : base(readModel)
        {
            this.UnitId = new UnitId(readModel.UnitId);
            this.MachineId = new MachineId(readModel.MachineId);
            this.BeamId = new BeamId(readModel.BeamId);
            this.OrderId = new OrderId(readModel.OrderId);
            this.DailyOperationStatus = readModel.DailyOperationStatus;
            this.DailyOperationMonitoringId =
                readModel.DailyOperationMonitoringId.HasValue ? 
                    new DailyOperationMonitoringId(readModel.DailyOperationMonitoringId.Value) : null;
            this.DailyOperationMachineDetails = 
                readModel.DailyOperationLoomDetails;
            this.UsedYarn = 
                readModel.UsedYarn.HasValue ? readModel.UsedYarn.Value : 0;
        }

        public void AddYarnUsed(double yarnLength)
        {
            if (UsedYarn != yarnLength)
            {
                UsedYarn = yarnLength;
                ReadModel.UsedYarn = UsedYarn;

                MarkModified();
            }
        }

        public void AddDailyOperationMachineDetail(DailyOperationLoomDetail dailyOperationMachineDetail)
        {
            var list = DailyOperationMachineDetails.ToList();
            list.Add(dailyOperationMachineDetail);
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
