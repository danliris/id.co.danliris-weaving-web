using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingBeamProduct : EntityBase<DailyOperationWarpingBeamProduct>
    {
        public Guid WarpingBeamId { get; private set; }
        public double? WarpingTotalBeamLength { get; private set; }
        public int? WarpingTotalBeamLengthUomId { get; private set; }
        public double? Tention { get; private set; }
        public int? MachineSpeed { get; private set; }
        public double? PressRoll { get; private set; }
        public int? PressRollUomId { get; private set; }
        public string BeamStatus { get; private set; }
        public DateTimeOffset LatestDateTimeBeamProduct{ get; private set; }
        public IReadOnlyCollection<DailyOperationWarpingBrokenCause> WarpingBrokenThreadsCauses { get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }
        public DailyOperationWarpingReadModel DailyOperationWarpingDocument { get; set; }

        public DailyOperationWarpingBeamProduct(Guid identity) : base(identity) { }

        public DailyOperationWarpingBeamProduct(Guid identity,
                                                BeamId warpingBeamId,
                                                DateTimeOffset latestDateTimeBeamProduct,
                                                string beamStatus) : base(identity)
        {
            Identity = identity;
            WarpingBeamId = warpingBeamId.Value;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            BeamStatus = beamStatus;
        }

        public void SetWarpingBeamId(Guid value)
        {
            if (!WarpingBeamId.Equals(value))
            {
                WarpingBeamId = value;

                MarkModified();
            }
        }

        public void SetWarpingTotalBeamLength(double value)
        {
            if (WarpingTotalBeamLength != value)
            {
                WarpingTotalBeamLength = value;

                MarkModified();
            }
        }

        public void SetWarpingBeamLengthUomId(int value)
        {
            if (WarpingTotalBeamLengthUomId != value)
            {
                WarpingTotalBeamLengthUomId = value;

                MarkModified();
            }
        }

        public void SetTention(double value)
        {
            if (Tention != value)
            {
                Tention = value;

                MarkModified();
            }
        }

        public void SetMachineSpeed(int value)
        {
            if (MachineSpeed != value)
            {
                MachineSpeed = value;

                MarkModified();
            }
        }

        public void SetPressRoll(double value)
        {
            if (PressRoll != value)
            {
                PressRoll = value;

                MarkModified();
            }
        }

        public void SetPressRollUomId(int value)
        {
            if (PressRollUomId != value)
            {
                PressRollUomId = value;

                MarkModified();
            }
        }

        public void SetBeamStatus(string value)
        {
            if (!BeamStatus.Equals(value))
            {
                BeamStatus = value;

                MarkModified();
            }
        }

        public void SetLatestDateTimeBeamProduct(DateTimeOffset value)
        {
            if (!LatestDateTimeBeamProduct.Equals(value))
            {
                LatestDateTimeBeamProduct = value;

                MarkModified();
            }
        }

        protected override DailyOperationWarpingBeamProduct GetEntity()
        {
            return this;
        }
    }
}
