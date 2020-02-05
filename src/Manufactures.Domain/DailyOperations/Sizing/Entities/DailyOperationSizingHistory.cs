using Infrastructure.Domain;
using Infrastructure.Domain.Events;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Sizing.Entities
{
    public class DailyOperationSizingHistory : AggregateRoot<DailyOperationSizingHistory, DailyOperationSizingHistoryReadModel>
    {
        public ShiftId ShiftDocumentId { get; private set; }
        public OperatorId OperatorDocumentId { get; private set; }
        public DateTimeOffset DateTimeMachine { get; private set; }
        public string MachineStatus { get; private set; }
        public string Information { get; private set; }
        public int BrokenBeam { get; private set; }
        public int MachineTroubled { get; private set; }
        public string SizingBeamNumber { get; private set; }
        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingHistory(Guid identity) : base(identity)
        {
        }

        //Constructor (Write)
        public DailyOperationSizingHistory(Guid identity,
                                           ShiftId shiftDocumentId,
                                           OperatorId operatorDocumentId,
                                           DateTimeOffset dateTimeMachine,
                                           string machineStatus,
                                           string information,
                                           int brokenBeam,
                                           int machineTroubled,
                                           string sizingBeamNumber,
                                           Guid dailyOperationSizingDocumentId) : base(identity)
        {
            Identity = identity;
            ShiftDocumentId = shiftDocumentId;
            OperatorDocumentId = operatorDocumentId;
            DateTimeMachine = dateTimeMachine;
            MachineStatus = machineStatus;
            Information = information;
            BrokenBeam = brokenBeam;
            MachineTroubled = machineTroubled;
            SizingBeamNumber = sizingBeamNumber;
            DailyOperationSizingDocumentId = dailyOperationSizingDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationSizingHistoryReadModel(Identity)
            {
                ShiftDocumentId = ShiftDocumentId.Value,
                OperatorDocumentId = OperatorDocumentId.Value,
                DateTimeMachine = DateTimeMachine,
                MachineStatus = MachineStatus,
                Information = Information,
                BrokenBeam = BrokenBeam,
                MachineTroubled = MachineTroubled,
                SizingBeamNumber = SizingBeamNumber,
                DailyOperationSizingDocumentId = DailyOperationSizingDocumentId
            };
        }

        //Constructor for Mapping Object from Database to Domain (Read)
        public DailyOperationSizingHistory(DailyOperationSizingHistoryReadModel readModel) : base(readModel)
        {
            //Instantiate object from database
            ShiftDocumentId = new ShiftId(readModel.ShiftDocumentId);
            OperatorDocumentId = new OperatorId(readModel.OperatorDocumentId);
            DateTimeMachine = readModel.DateTimeMachine;
            MachineStatus = readModel.MachineStatus;
            Information = readModel.Information;
            BrokenBeam = readModel.BrokenBeam;
            MachineTroubled = readModel.MachineTroubled;
            SizingBeamNumber = readModel.SizingBeamNumber;
            DailyOperationSizingDocumentId = readModel.DailyOperationSizingDocumentId;
        }

        public void SetShiftId(ShiftId shiftDocumentId)
        {
            Validator.ThrowIfNull(() => shiftDocumentId);
            if (shiftDocumentId != ShiftDocumentId)
            {
                ShiftDocumentId = shiftDocumentId;
                ReadModel.ShiftDocumentId = ShiftDocumentId.Value;

                MarkModified();
            }
        }

        public void SetOperatorDocumentId(OperatorId operatorDocumentId)
        {
            Validator.ThrowIfNull(() => operatorDocumentId);
            if (operatorDocumentId != OperatorDocumentId)
            {
                OperatorDocumentId = operatorDocumentId;
                ReadModel.OperatorDocumentId = OperatorDocumentId.Value;

                MarkModified();
            }
        }

        public void SetDateTimeMachine(DateTimeOffset dateTimeMachine)
        {
            if (dateTimeMachine != DateTimeMachine)
            {
                DateTimeMachine = dateTimeMachine;
                ReadModel.DateTimeMachine = DateTimeMachine;

                MarkModified();
            }
        }

        public void SetMachineStatus(string machineStatus)
        {
            Validator.ThrowIfNull(() => machineStatus);
            if (machineStatus != MachineStatus)
            {
                MachineStatus = machineStatus;
                ReadModel.MachineStatus = MachineStatus;

                MarkModified();
            }
        }

        public void SetInformation(string information)
        {
            Validator.ThrowIfNull(() => information);
            if (information != Information)
            {
                Information = information;
                ReadModel.Information = Information;

                MarkModified();
            }
        }

        public void SetBrokenBeam(int brokenBeam)
        {
            if (brokenBeam != BrokenBeam)
            {
                BrokenBeam = brokenBeam;
                ReadModel.BrokenBeam = BrokenBeam;

                MarkModified();
            }
        }

        public void SetMachineTroubled(int machineTroubled)
        {
            if (machineTroubled != MachineTroubled)
            {
                MachineTroubled = machineTroubled;
                ReadModel.MachineTroubled = MachineTroubled;

                MarkModified();
            }
        }

        public void SetSizingBeamNumber(string sizingBeamNumber)
        {
            Validator.ThrowIfNull(() => sizingBeamNumber);
            if (sizingBeamNumber != SizingBeamNumber)
            {
                SizingBeamNumber = sizingBeamNumber;
                ReadModel.SizingBeamNumber = SizingBeamNumber;

                MarkModified();
            }
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override DailyOperationSizingHistory GetEntity()
        {
            return this;
        }

        //protected override void MarkRemoved()
        //{
        //    DeletedBy = "System";
        //    Deleted = true;
        //    DeletedDate = DateTimeOffset.UtcNow;

        //    if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityDeleted<DailyOperationSizingHistory>))
        //        this.AddDomainEvent(new OnEntityDeleted<DailyOperationSizingHistory>(GetEntity()));

        //    // clear updated events
        //    if (this.DomainEvents.Any(o => o is OnEntityUpdated<DailyOperationSizingHistory>))
        //    {
        //        this.DomainEvents.Where(o => o is OnEntityUpdated<DailyOperationSizingHistory>)
        //            .ToList()
        //            .ForEach(o => this.RemoveDomainEvent(o));
        //    }
        //}
    }
}
