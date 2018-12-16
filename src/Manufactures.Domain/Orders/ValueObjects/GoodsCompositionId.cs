using Moonlay;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class GoodsCompositionId : SingleValueObject<string>
    {
        public GoodsCompositionId(string value) : base(value)
        {
            if(!string.IsNullOrEmpty(value) && !Guid.TryParse(value, out Guid id))
            {
                throw Validator.ErrorValidation(("GoodsCompositionId", "Invalid GUID"));
            }
        }
    }
}