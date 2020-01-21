using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingBeamProduct : AggregateRoot<DailyOperationWarpingBeamProduct, DailyOperationWarpingBeamProductReadModel>
    {
        public BeamId WarpingBeamId { get; private set; }
        public double WarpingTotalBeamLength { get; private set; }
        public int WarpingTotalBeamLengthUomId { get; private set; }
        public double? Tention { get; private set; }
        public int? MachineSpeed { get; private set; }
        public double? PressRoll { get; private set; }
        public string PressRollUom { get; private set; }
        public string BeamStatus { get; private set; }
        public DateTimeOffset LatestDateTimeBeamProduct { get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }
        public List<DailyOperationWarpingBrokenCause> BrokenCauses { get; set; }
        //Constructor (Write)
        public DailyOperationWarpingBeamProduct(Guid identity,
                                                BeamId warpingBeamId,
                                                DateTimeOffset latestDateTimeBeamProduct,
                                                string beamStatus,
                                                Guid dailyOperationWarpingDocumentId) : base(identity)
        {
            Identity = identity;
            WarpingBeamId = warpingBeamId;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            BeamStatus = beamStatus;
            DailyOperationWarpingDocumentId = dailyOperationWarpingDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationWarpingBeamProductReadModel(Identity)
            {

                WarpingBeamId = WarpingBeamId.Value,
                WarpingTotalBeamLength = WarpingTotalBeamLength,
                WarpingTotalBeamLengthUomId = WarpingTotalBeamLengthUomId,
                Tention = Tention ?? 0,
                MachineSpeed = MachineSpeed ?? 0,
                PressRoll = PressRoll ?? 0,
                PressRollUom = PressRollUom,
                BeamStatus = BeamStatus,
                LatestDateTimeBeamProduct = LatestDateTimeBeamProduct,
                DailyOperationWarpingDocumentId = dailyOperationWarpingDocumentId
            };
        }

        //Constructor for Mapping Object from Database to Domain (Read)
        public DailyOperationWarpingBeamProduct(DailyOperationWarpingBeamProductReadModel readModel) : base(readModel)
        {
            //Instantiate Object from Database
            WarpingBeamId = new BeamId(readModel.WarpingBeamId);
            LatestDateTimeBeamProduct = readModel.LatestDateTimeBeamProduct;
            BeamStatus = readModel.BeamStatus;
            DailyOperationWarpingDocumentId = readModel.DailyOperationWarpingDocumentId;
            WarpingTotalBeamLength = readModel.WarpingTotalBeamLength;
            WarpingTotalBeamLengthUomId = readModel.WarpingTotalBeamLengthUomId;
            Tention = readModel.Tention;
            MachineSpeed = readModel.MachineSpeed;
            PressRoll = readModel.PressRoll;
            PressRollUom = readModel.PressRollUom;
        }

        public void SetWarpingBeamId(BeamId warpingBeamId)
        {
            Validator.ThrowIfNull(() => warpingBeamId);

            if (warpingBeamId != WarpingBeamId)
            {
                WarpingBeamId = warpingBeamId;
                ReadModel.WarpingBeamId = warpingBeamId.Value;

                MarkModified();
            }
        }

        public void SetWarpingTotalBeamLength(double warpingTotalBeamLength)
        {
            if (WarpingTotalBeamLength != warpingTotalBeamLength)
            {
                WarpingTotalBeamLength = warpingTotalBeamLength;
                ReadModel.WarpingTotalBeamLength = warpingTotalBeamLength;

                MarkModified();
            }
        }

        public void SetWarpingTotalBeamLengthUomId(UomId warpingTotalBeamLengthUomId)
        {
            if (WarpingTotalBeamLengthUomId != warpingTotalBeamLengthUomId.Value)
            {
                WarpingTotalBeamLengthUomId = warpingTotalBeamLengthUomId.Value;
                ReadModel.WarpingTotalBeamLengthUomId = WarpingTotalBeamLengthUomId;

                MarkModified();
            }
        }

        public void SetTention(double tention)
        {
            if (Tention != tention)
            {
                Tention = tention;
                ReadModel.Tention = tention;

                MarkModified();
            }
        }

        public void SetMachineSpeed(int machineSpeed)
        {
            if (MachineSpeed != machineSpeed)
            {
                MachineSpeed = machineSpeed;
                ReadModel.MachineSpeed = machineSpeed;

                MarkModified();
            }
        }

        public void SetPressRoll(double pressRoll)
        {
            if (PressRoll != pressRoll)
            {
                PressRoll = pressRoll;
                ReadModel.PressRoll = pressRoll;

                MarkModified();
            }
        }

        public void SetPressRollUom(string pressRollUom)
        {
            Validator.ThrowIfNull(() => pressRollUom);

            if (PressRollUom != pressRollUom)
            {
                PressRollUom = pressRollUom;
                ReadModel.PressRollUom = pressRollUom;

                MarkModified();
            }
        }

        public void SetBeamStatus(string beamStatus)
        {
            Validator.ThrowIfNull(() => beamStatus);

            if (beamStatus != BeamStatus)
            {
                BeamStatus = beamStatus;
                ReadModel.BeamStatus = beamStatus;

                MarkModified();
            }
        }

        public void SetLatestDateTimeBeamProduct(DateTimeOffset latestDateTimeBeamProduct)
        {
            if (latestDateTimeBeamProduct != LatestDateTimeBeamProduct)
            {
                LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
                ReadModel.LatestDateTimeBeamProduct = latestDateTimeBeamProduct;

                MarkModified();
            }
        }

        ////Add Warping Broken Threads Causes
        //public void AddWarpingBrokenThreadsCause(DailyOperationWarpingBrokenCause cause)
        //{
        //    //Modified Existing List of Detail
        //    var dailyOperationWarpingBrokenCauses = WarpingBrokenThreadsCauses.ToList();

        //    //Add New Detail
        //    dailyOperationWarpingBrokenCauses.Add(cause);

        //    //Update Old List
        //    WarpingBrokenThreadsCauses = dailyOperationWarpingBrokenCauses;

        //    MarkModified();
        //}

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override DailyOperationWarpingBeamProduct GetEntity()
        {
            return this;
        }
    }
}
