using Manufactures.Domain.YarnNumbers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.YarnNumber
{
    public class YarnNumberListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; }

        [JsonProperty(PropertyName = "Number")]
        public int Number { get; }

        [JsonProperty(PropertyName = "RingType")]
        public string RingType { get; }

        public YarnNumberListDto(YarnNumberDocument ringDocument)
        {
            Id = ringDocument.Identity;
            Code = ringDocument.Code;
            Number = ringDocument.Number;
            RingType = ringDocument.RingType;
        }
    }
}
