using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class ShiftDocumentValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Identity")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; set; }

        public ShiftDocumentValueObject(Guid identity, string shiftName)
        {
            Identity = identity;
            ShiftName = shiftName;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identity;
            yield return ShiftName;
        }
    }
}
