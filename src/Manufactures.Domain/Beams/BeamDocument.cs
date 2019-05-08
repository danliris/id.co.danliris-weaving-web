using Infrastructure.Domain;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Events;
using System;

namespace Manufactures.Domain.Beams
{
    public class BeamDocument : AggregateRoot<BeamDocument, BeamReadModel>
    {
        public string Number { get; private set; }
        public string Type { get; private set; }
        public double EmptyWeight { get; private set; }

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

        protected override BeamDocument GetEntity()
        {
            return this;
        }
    }
}
