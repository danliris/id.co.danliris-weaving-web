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
        public Guid BeamDocumentId { get; private set; }

        public Guid MachineDocumentId { get; private set; }

        public Guid OperatorDocumentId { get; private set; }

        public DateTimeOffset DateTimeMachine { get; private set; }

        public Guid ShiftDocumentId { get; private set; }

        public string Process { get; private set; }

        public double? GreigeLength { get; private set; }

        public string Information { get; private set; }

        public string MachineStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomReadModel DailyOperationLoomDocument { get; set; }

        public DailyOperationLoomBeamHistory(Guid identity) : base(identity)
        {
        }

        public DailyOperationLoomBeamHistory(Guid identity,
                                             BeamId beamDocumentId, 
                                             MachineId machineDocumentId, 
                                             OperatorId operatorDocumentId, 
                                             DateTimeOffset dateTimeMachine, 
                                             ShiftId shiftDocumentId,
                                             string process,
                                             string information, 
                                             string machineStatus) : base(identity)
        {
            Identity = identity;
            BeamDocumentId = beamDocumentId.Value;
            MachineDocumentId = machineDocumentId.Value;
            OperatorDocumentId = operatorDocumentId.Value;
            DateTimeMachine = dateTimeMachine;
            ShiftDocumentId = shiftDocumentId.Value;
            Process = process;
            Information = information;
            MachineStatus = machineStatus;
        }

        public void SetBeamDocumentId(BeamId beamDocumentId)
        {
            BeamDocumentId = beamDocumentId.Value;
            MarkModified();
        }

        public void SetMachineDocumentId(MachineId machineDocumentId)
        {
            MachineDocumentId = machineDocumentId.Value;
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

        public void SetProcess(string process)
        {
            Process = process;
            MarkModified();
        }

        public void SetGreigeLength(double greigeLength)
        {
            GreigeLength = greigeLength;
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
