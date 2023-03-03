using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class SupplierId : SingleValueObject<Guid>
    {
        public SupplierId(Guid id) : base(id) { }
    }
}
