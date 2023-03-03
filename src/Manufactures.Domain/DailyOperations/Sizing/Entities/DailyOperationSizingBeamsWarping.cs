using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.DailyOperations.Sizing.Entities
{
    public class DailyOperationSizingBeamsWarping : AggregateRoot<DailyOperationSizingBeamsWarping, DailyOperationSizingBeamsWarpingReadModel>
    {
        public BeamId BeamDocumentId { get; private set; }
        public double YarnStrands { get; private set; }
        public double EmptyWeight { get; private set; }
        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingBeamsWarping(Guid identity,
                                                BeamId beamDocumentId,
                                                double yarnStrands,
                                                double emptyWeight,
                                                Guid dailyOperationSizingDocumentId) : base(identity)
        {
            Identity = identity;
            BeamDocumentId = beamDocumentId;
            YarnStrands = yarnStrands;
            EmptyWeight = emptyWeight;
            DailyOperationSizingDocumentId = dailyOperationSizingDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationSizingBeamsWarpingReadModel(Identity)
            {
                BeamDocumentId = BeamDocumentId.Value,
                YarnStrands = YarnStrands,
                EmptyWeight = EmptyWeight,
                DailyOperationSizingDocumentId = DailyOperationSizingDocumentId
            };
        }

        public DailyOperationSizingBeamsWarping(DailyOperationSizingBeamsWarpingReadModel readModel) : base(readModel)
        {
            BeamDocumentId = new BeamId(readModel.BeamDocumentId);
            YarnStrands = readModel.YarnStrands;
            EmptyWeight = readModel.EmptyWeight;
            DailyOperationSizingDocumentId = readModel.DailyOperationSizingDocumentId;
        }

        public void SetBeamDocumentId(BeamId beamDocumentId)
        {
            Validator.ThrowIfNull(() => beamDocumentId);
            if (beamDocumentId != BeamDocumentId)
            {
                BeamDocumentId = beamDocumentId;
                ReadModel.BeamDocumentId = BeamDocumentId.Value;

                MarkModified();
            }
        }

        public void SetYarnStrands(double yarnStrands)
        {
            if (yarnStrands != YarnStrands)
            {
                YarnStrands = yarnStrands;
                ReadModel.YarnStrands = YarnStrands;

                MarkModified();
            }
        }

        public void SetEmptyWeight(double emptyWeight)
        {
            if (emptyWeight != EmptyWeight)
            {
                EmptyWeight = emptyWeight;
                ReadModel.EmptyWeight = EmptyWeight;

                MarkModified();
            }
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override DailyOperationSizingBeamsWarping GetEntity()
        {
            return this;
        }
    }
}
