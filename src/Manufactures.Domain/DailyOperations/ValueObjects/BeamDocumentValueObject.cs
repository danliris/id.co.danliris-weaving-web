using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class BeamDocumentValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Identity")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(PropertyName = "SizingNumber")]
        public string SizingNumber { get; set; }

        public BeamDocumentValueObject(Guid identity, string beamNumber, string sizingNumber)
        {
            Identity = identity;
            BeamNumber = beamNumber;
            SizingNumber = sizingNumber;
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identity;
            yield return BeamNumber;
            yield return SizingNumber;
        }
    }
}
