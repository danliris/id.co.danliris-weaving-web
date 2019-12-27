using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class BrokenCauseId : SingleValueObject<Guid>
    {
        public BrokenCauseId(Guid id) : base(id) { }
    }
}
