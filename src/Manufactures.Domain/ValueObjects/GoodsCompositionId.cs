using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class GoodsCompositionId : SingleValueObject<string>
    {
        public GoodsCompositionId(string value) : base(value)
        {
        }
    }
}
