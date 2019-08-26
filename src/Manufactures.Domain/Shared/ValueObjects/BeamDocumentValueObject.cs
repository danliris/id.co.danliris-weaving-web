using Manufactures.Domain.Beams;
using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class BeamDocumentValueObject : ValueObject
    {
        public Guid Identity { get; private set; }
        public string Number { get; private set; }
        public string Type { get; private set; }
        public double EmptyWeight { get; private set; }
        public double YarnLength { get; private set; }
        public double YarnStrands { get; private set; }

        public BeamDocumentValueObject(BeamDocument beamDocument)
        {
            Identity = beamDocument.Identity;
            Number = beamDocument.Number;
            Type = beamDocument.Type;
            EmptyWeight = beamDocument.EmptyWeight;
            YarnLength = beamDocument.YarnLength;
            YarnStrands = beamDocument.YarnStrands;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identity;
            yield return Number;
            yield return Type;
            yield return EmptyWeight;
            yield return YarnLength;
            yield return YarnStrands;
        }
    }
}
