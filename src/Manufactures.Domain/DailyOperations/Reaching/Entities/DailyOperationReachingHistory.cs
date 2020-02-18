using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Entities
{
    public class DailyOperationReachingHistory : AggregateRoot<DailyOperationReachingHistory, DailyOperationReachingHistoryReadModel>
    {
        public OperatorId OperatorDocumentId { get; private set; }

        public int YarnStrandsProcessed { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public ShiftId ShiftDocumentId { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationReachingDocumentId { get; set; }

        public DailyOperationReachingHistory(Guid identity, OperatorId operatorDocumentId, int yarnStrandsProcessed, DateTimeOffset dateTimeMachine, ShiftId shiftDocumentId, string machineStatus, Guid dailyOperationReachingDocumentId) : base(identity)
        {
            MarkTransient();
            
            Identity = identity;
            OperatorDocumentId = operatorDocumentId;
            YarnStrandsProcessed = yarnStrandsProcessed;
            DateTimeMachine = dateTimeMachine;
            ShiftDocumentId = shiftDocumentId;
            MachineStatus = machineStatus;
            DailyOperationReachingDocumentId = dailyOperationReachingDocumentId;

            ReadModel = new DailyOperationReachingHistoryReadModel(Identity)
            {
                OperatorDocumentId = OperatorDocumentId.Value,
                YarnStrandsProcessed = YarnStrandsProcessed,
                DateTimeMachine = DateTimeMachine,
                ShiftDocumentId = ShiftDocumentId.Value,
                MachineStatus = MachineStatus,
                DailyOperationReachingDocumentId = DailyOperationReachingDocumentId
            };

            ReadModel.AddDomainEvent(new OnAddDailyOperationReachingHistory(Identity));
        }

        public DailyOperationReachingHistory(DailyOperationReachingHistoryReadModel readModel) : base(readModel)
        {
            OperatorDocumentId = new OperatorId(readModel.OperatorDocumentId);
            YarnStrandsProcessed = readModel.YarnStrandsProcessed;
            DateTimeMachine = readModel.DateTimeMachine;
            ShiftDocumentId = new ShiftId(readModel.ShiftDocumentId);
            MachineStatus = readModel.MachineStatus;
            DailyOperationReachingDocumentId = readModel.DailyOperationReachingDocumentId;
        }

        protected override DailyOperationReachingHistory GetEntity()
        {
            return this;
        }

        public void SetOperatorDocumentId(OperatorId newOperatorDocumentId)
        {
            if (OperatorDocumentId != newOperatorDocumentId)
            {
                OperatorDocumentId = newOperatorDocumentId;
                ReadModel.OperatorDocumentId = OperatorDocumentId.Value;
                MarkModified();
            }
        }

        public void SetYarnStrandsProcessed(int newYarnStrandsProcessed)
        {
            if(YarnStrandsProcessed != newYarnStrandsProcessed)
            {
                YarnStrandsProcessed = newYarnStrandsProcessed;
                ReadModel.YarnStrandsProcessed = YarnStrandsProcessed;
                MarkModified();
            }
        }

        public void SetDateTimeMachine(DateTimeOffset newDateTimeMachine)
        {
            if(DateTimeMachine != newDateTimeMachine)
            {
                DateTimeMachine = newDateTimeMachine;
                ReadModel.DateTimeMachine = DateTimeMachine;
                MarkModified();
            }
        }

        public void SetShiftId(ShiftId newShiftDocumentId)
        {
            if(ShiftDocumentId != newShiftDocumentId)
            {
                ShiftDocumentId = newShiftDocumentId;
                ReadModel.ShiftDocumentId = ShiftDocumentId.Value;
                MarkModified();
            }
            
        }

        public void SetMachineStatus(string newMachineStatus)
        {
            if(MachineStatus != newMachineStatus)
            {
                MachineStatus = newMachineStatus;
                ReadModel.MachineStatus = MachineStatus;
                MarkModified();
            }
            
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }
    }
}
