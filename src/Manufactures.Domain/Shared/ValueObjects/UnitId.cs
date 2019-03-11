using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shared.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class UnitId : SingleValueObject<int>
    {
        public UnitId(int id) : base(id) { }
    }
}
