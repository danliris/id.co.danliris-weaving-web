using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingHistory : AggregateRoot<DailyOperationWarpingHistory, DailyOperationWarpingHistoryReadModel>
    {
        public ShiftId ShiftDocumentId { get; private set; }
        public OperatorId OperatorDocumentId { get; private set; }
        public DateTimeOffset DateTimeMachine { get; private set; }
        public string MachineStatus { get; private set; }
        public string Information { get; private set; }
        public BeamId WarpingBeamId { get; private set; }
        public double WarpingBeamLengthPerOperator { get; private set; }
        public UomId WarpingBeamLengthPerOperatorUomId { get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }

        public DailyOperationWarpingHistory(Guid identity) : base(identity)
        {
        }

        //Constructor (Write)
        public DailyOperationWarpingHistory(Guid identity,
                                            ShiftId shiftDocumentId,
                                            OperatorId operatorDocumentId,
                                            DateTimeOffset dateTimeMachine,
                                            string machineStatus,
                                            Guid dailyOperationWarpingDocumentId) : base(identity)
        {
            //Instantiate Properties from Parameter Variable
            Identity = identity;
            ShiftDocumentId = shiftDocumentId;
            OperatorDocumentId = operatorDocumentId;
            DateTimeMachine = dateTimeMachine;
            MachineStatus = machineStatus;
            DailyOperationWarpingDocumentId = dailyOperationWarpingDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationWarpingHistoryReadModel(Identity)
            {
                ShiftDocumentId = ShiftDocumentId.Value,
                OperatorDocumentId = OperatorDocumentId.Value,
                DateTimeMachine = DateTimeMachine,
                MachineStatus = MachineStatus,
                DailyOperationWarpingDocumentId = DailyOperationWarpingDocumentId
            };
        }

        //Constructor for Mapping Object from Database to Domain (Read)
        public DailyOperationWarpingHistory(DailyOperationWarpingHistoryReadModel readModel) : base(readModel)
        {
            //Instantiate object from database
            ShiftDocumentId = new ShiftId(readModel.ShiftDocumentId);
            OperatorDocumentId = new OperatorId(readModel.OperatorDocumentId);
            DateTimeMachine = readModel.DateTimeMachine;
            MachineStatus = readModel.MachineStatus;
            Information = readModel.Information;
            WarpingBeamId = new BeamId(readModel.WarpingBeamId);
            WarpingBeamLengthPerOperator = readModel.WarpingBeamLengthPerOperator;
            WarpingBeamLengthPerOperatorUomId = new UomId(readModel.WarpingBeamLengthPerOperatorUomId);
            DailyOperationWarpingDocumentId = readModel.DailyOperationWarpingDocumentId;
        }

        public void SetShiftDocumentId(ShiftId shiftDocumentId)
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

        public void SetWarpingBeamId(BeamId warpingBeamId)
        {
            Validator.ThrowIfNull(() => warpingBeamId);

            if (warpingBeamId != WarpingBeamId)
            {
                WarpingBeamId = warpingBeamId;
                ReadModel.WarpingBeamId = WarpingBeamId.Value;

                MarkModified();
            }
        }

        public void SetWarpingBeamLengthPerOperator(double warpingBeamLengthPerOperator)
        {
            if (warpingBeamLengthPerOperator != WarpingBeamLengthPerOperator)
            {
                WarpingBeamLengthPerOperator = warpingBeamLengthPerOperator;
                ReadModel.WarpingBeamLengthPerOperator = WarpingBeamLengthPerOperator;

                MarkModified();
            }
        }

        public void SetWarpingBeamLengthPerOperatorUomId(UomId warpingBeamLengthPerOperatorUomId)
        {
            if (warpingBeamLengthPerOperatorUomId != WarpingBeamLengthPerOperatorUomId)
            {
                WarpingBeamLengthPerOperatorUomId = warpingBeamLengthPerOperatorUomId;
                ReadModel.WarpingBeamLengthPerOperatorUomId = WarpingBeamLengthPerOperatorUomId.Value;

                MarkModified();
            }
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override DailyOperationWarpingHistory GetEntity()
        {
            return this;
        }
    }
}
