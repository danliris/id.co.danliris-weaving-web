using Infrastructure.Domain;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Events;
using System;

namespace Manufactures.Domain.Beams
{
    public class BeamDocument : AggregateRoot<BeamDocument, BeamReadModel>
    {
        public string BeamNumber { get; private set; }
        public string BeamType { get; private set; }

        public BeamDocument(Guid identity,
                            string beamNumber,
                            string beamType) : base(identity)
        {
            Identity = identity;
            BeamNumber = beamNumber;
            BeamType = beamType;

            ReadModel = new BeamReadModel(Identity)
            {
                BeamNumber = BeamNumber,
                BeamType = BeamType
            };

            MarkTransient();

            ReadModel.AddDomainEvent(new OnAddBeam(Identity));
        }

        public BeamDocument(BeamReadModel readModel) : base(readModel)
        {
            this.BeamNumber = readModel.BeamNumber;
            this.BeamType = readModel.BeamType;
        }

        public void SetBeamNumber(string value)
        {
            if(BeamNumber != value)
            {
                BeamNumber = value;
                ReadModel.BeamNumber = BeamNumber;

                MarkModified();
            }
        }

        public void SetBeamType(string value)
        {
            if (BeamType != value)
            {
                BeamNumber = value;
                ReadModel.BeamNumber = BeamNumber;

                MarkModified();
            }
        }

        protected override BeamDocument GetEntity()
        {
            return this;
        }
    }
}
