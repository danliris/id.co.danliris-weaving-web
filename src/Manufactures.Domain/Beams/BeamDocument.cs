﻿using Infrastructure.Domain;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.Beams
{
    public class BeamDocument : AggregateRoot<BeamDocument, BeamReadModel>
    {
        public string Number { get; private set; }
        public string Type { get; private set; }
        public double EmptyWeight { get; private set; }
        public double YarnLength { get; private set; }
        public ConstructionId ConstructionId { get; private set; }

        public BeamDocument(Guid identity,
                            string beamNumber,
                            string beamType,
                            double emptyWeight) : base(identity)
        {
            Identity = identity;
            Number = beamNumber;
            Type = beamType;
            EmptyWeight = emptyWeight;

            ReadModel = new BeamReadModel(Identity)
            {
                Number = Number,
                Type = Type,
                EmtpyWeight = EmptyWeight
            };

            MarkTransient();

            ReadModel.AddDomainEvent(new OnAddBeam(Identity));
        }

        public BeamDocument(BeamReadModel readModel) : base(readModel)
        {
            this.Number = readModel.Number;
            this.Type = readModel.Type;
            this.EmptyWeight = readModel.EmtpyWeight;
            this.ConstructionId = readModel.ContructionId.HasValue ? new ConstructionId(readModel.ContructionId.Value) : null;
            this.YarnLength = readModel.YarnLength.HasValue ? readModel.YarnLength.Value : 0;
        }

        public void SetBeamNumber(string value)
        {
            if (Number != value)
            {
                Number = value;
                ReadModel.Number = Number;

                MarkModified();
            }
        }

        public void SetBeamType(string value)
        {
            if (Type != value)
            {
                Number = value;
                ReadModel.Number = Number;

                MarkModified();
            }
        }

        public void SetEmptyWeight(double value)
        {
            if (EmptyWeight != value)
            {
                EmptyWeight = value;
                ReadModel.EmtpyWeight = EmptyWeight;

                MarkModified();
            }
        }

        public void SetLatestConstructionId( ConstructionId constructionId)
        {
            if (!ConstructionId.Value.Equals(constructionId.Value))
            {
                ConstructionId = constructionId;
                ReadModel.ContructionId = ConstructionId.Value;

                MarkModified();
            }
        }

        public void SetLatestYarnLength(double yarnLength)
        {
            if (YarnLength != yarnLength)
            {
                YarnLength = yarnLength;
                ReadModel.YarnLength = YarnLength;

                MarkModified();
            }
        }

        protected override BeamDocument GetEntity()
        {
            return this;
        }
    }
}
