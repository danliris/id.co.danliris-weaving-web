using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingBeamProduct
        : EntityBase<DailyOperationWarpingBeamProduct>
    {
        public Guid WarpingBeamId { get; private set; }
        public string BrokenThreadsCause { get; private set; }
        public int? ConeDeficient { get; private set; }
        public int? LooseThreadsAmount { get; private set; }
        public int? RightLooseCreel { get; private set; }
        public int? LeftLooseCreel { get; private set; }
        public double? WarpingBeamLength { get; private set; }
        public int? Tention { get; private set; }
        public int? MachineSpeed { get; private set; }
        public double? PressRoll { get; private set; }
        public string BeamStatus { get; private set; }
        public DateTimeOffset DateTimeBeamProduct{ get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }
        public DailyOperationWarpingReadModel DailyOperationWarpingDocument { get; set; }

        public DailyOperationWarpingBeamProduct(Guid identity) : base(identity) { }

        public DailyOperationWarpingBeamProduct(Guid identity,
                                                BeamId warpingBeamId)
            : base(identity)
        {
            Identity = identity;
            WarpingBeamId = warpingBeamId.Value;
        }

        public void SetBrokenThreadsCause(string value)
        {
            if (BrokenThreadsCause != value)
            {
                BrokenThreadsCause = value;

                MarkModified();
            }
        }

        public void SetConeDeficient(int value)
        {
            if (ConeDeficient != value)
            {
                ConeDeficient = value;

                MarkModified();
            }
        }

        public void SetLooseThreadsAmount(int value)
        {
            if (LooseThreadsAmount != value)
            {
                LooseThreadsAmount = value;

                MarkModified();
            }
        }

        public void SetRightLooseCreel(int value)
        {
            if (RightLooseCreel != value)
            {
                RightLooseCreel = value;

                MarkModified();
            }
        }

        public void SetLeftLooseCreel(int value)
        {
            if (LeftLooseCreel != value)
            {
                LeftLooseCreel = value;

                MarkModified();
            }
        }

        public void SetWarpingBeamId(Guid value)
        {
            if (!WarpingBeamId.Equals(value))
            {
                WarpingBeamId = value;

                MarkModified();
            }
        }

        public void SetWarpingBeamLength(double value)
        {
            if (WarpingBeamLength != value)
            {
                WarpingBeamLength = value;

                MarkModified();
            }
        }

        public void SetTention(int value)
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

        public void SetBeamStatus(string value)
        {
            if (!BeamStatus.Equals(value))
            {
                BeamStatus = value;

                MarkModified();
            }
        }

        public void SetDateTimeBeamProduct(DateTimeOffset value)
        {
            if (!DateTimeBeamProduct.Equals(value))
            {
                DateTimeBeamProduct = value;

                MarkModified();
            }
        }

        protected override DailyOperationWarpingBeamProduct GetEntity()
        {
            return this;
        }
    }
}
