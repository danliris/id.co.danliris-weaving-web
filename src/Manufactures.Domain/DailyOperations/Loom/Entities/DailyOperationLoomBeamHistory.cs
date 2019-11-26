using Infrastructure.Domain;
using Infrastructure.Domain.Events;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomBeamHistory : EntityBase<DailyOperationLoomBeamHistory>
    {
        public string BeamNumber { get; private set; }

        public string MachineNumber { get; private set; }

        public Guid OperatorDocumentId { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public Guid ShiftDocumentId { get; private set; }

        public int? WarpBrokenThreads { get; private set; }

        public int? WeftBrokenThreads { get; private set; }

        public int? LenoBrokenThreads { get; private set; }

        public string ReprocessTo { get; private set; }

        public string Information { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomReadModel DailyOperationLoomDocument { get; set; }

        public DailyOperationLoomBeamHistory(Guid identity) : base(identity)
        {
        }

        public DailyOperationLoomBeamHistory(Guid identity,
                                             string beamNumber,
                                             string machineNumber,
                                             OperatorId operatorDocumentId, 
                                             DateTimeOffset dateTimeMachine, 
                                             ShiftId shiftDocumentId,
                                             string information, 
                                             string machineStatus) : base(identity)
        {
            BeamNumber = beamNumber;
            MachineNumber = machineNumber;
            OperatorDocumentId = operatorDocumentId.Value;
            DateTimeMachine = dateTimeMachine;
            ShiftDocumentId = shiftDocumentId.Value;
            Information = information;
            MachineStatus = machineStatus;
        }

        public void SetBeamNumber(string beamNumber)
        {
            BeamNumber = beamNumber;
            MarkModified();
        }

        public void SetMachineNumber(string machineNumber)
        {
            MachineNumber = machineNumber;
            MarkModified();
        }

        public void SetOperatorDocumentId(OperatorId operatorDocumentId)
        {
            OperatorDocumentId = operatorDocumentId.Value;
            MarkModified();
        }

        public void SetDateTimeMachine(DateTimeOffset dateTimeMachine)
        {
            DateTimeMachine = dateTimeMachine;
            MarkModified();
        }

        public void SetShiftDocumentId(ShiftId shiftDocumentId)
        {
            ShiftDocumentId = shiftDocumentId.Value;
            MarkModified();
        }

        public void SetWarpBrokenThreads(int warpBrokenThreads)
        {
            WarpBrokenThreads = warpBrokenThreads;
            MarkModified();
        }

        public void SetWeftBrokenThreads(int weftBrokenThreads)
        {
            WeftBrokenThreads = weftBrokenThreads;
            MarkModified();
        }

        public void SetLenoBrokenThreads(int lenoBrokenThreads)
        {
            LenoBrokenThreads= lenoBrokenThreads;
            MarkModified();
        }

        public void SetReprocessTo(string reprocessTo)
        {
            ReprocessTo = reprocessTo;
            MarkModified();
        }

        public void SetInformation(string information)
        {
            Information = information;
            MarkModified();
        }

        public void SetMachineStatus(string machineStatus)
        {
            MachineStatus = machineStatus;
            MarkModified();
        }

        protected override DailyOperationLoomBeamHistory GetEntity()
        {
            return this;
        }

        protected override void MarkRemoved()
        {
            DeletedBy = "System";
            Deleted = true;
            DeletedDate = DateTimeOffset.UtcNow;

            if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityDeleted<DailyOperationLoomBeamHistory>))
                this.AddDomainEvent(new OnEntityDeleted<DailyOperationLoomBeamHistory>(GetEntity()));

            // clear updated events
            if (this.DomainEvents.Any(o => o is OnEntityUpdated<DailyOperationLoomBeamHistory>))
            {
                this.DomainEvents.Where(o => o is OnEntityUpdated<DailyOperationLoomBeamHistory>)
                    .ToList()
                    .ForEach(o => this.RemoveDomainEvent(o));
            }
        }
    }
}
