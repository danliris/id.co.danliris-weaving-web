using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Entities
{
    public class DailyOperationWarpingBeamProduct
        : EntityBase<DailyOperationWarpingBeamProduct>
    {
        public Guid BeamId { get; private set; }
        public double? Length { get; private set; }
        public int? Tention { get; private set; }
        public int? Speed { get; private set; }
        public double? PressRoll { get; private set; }
        public string BeamStatus { get; private set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }
        public DailyOperationWarpingReadModel DailyOperationWarpingDocument { get; set; }

        public DailyOperationWarpingBeamProduct(Guid identity) : base(identity) { }

        public DailyOperationWarpingBeamProduct(Guid identity,
                                                BeamId beamId)
            : base(identity)
        {
            Identity = identity;
            BeamId = beamId.Value;
        }

        public void SetBeamId(Guid value)
        {
            if (!BeamId.Equals(value))
            {
                BeamId = value;

                MarkModified();
            }
        }

        public void SetLength(double value)
        {
            if (Length != value)
            {
                Length = value;

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

        public void SetSpeed(int value)
        {
            if (Speed != value)
            {
                Speed = value;

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

        public void UpdateBeamStatus(string value)
        {
            if (!BeamStatus.Equals(value))
            {
                BeamStatus = value;

                MarkModified();
            }
        }

        protected override DailyOperationWarpingBeamProduct GetEntity()
        {
            return this;
        }
    }
}
