using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class YarnDefectId : SingleValueObject<Guid>
    {
        public YarnDefectId(Guid id) : base(id) { }
    }
}
