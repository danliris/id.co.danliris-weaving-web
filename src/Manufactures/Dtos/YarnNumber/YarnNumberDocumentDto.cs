using Manufactures.Domain.YarnNumbers;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.YarnNumber
{
    public class YarnNumberDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Number")]
        public int Number { get; private set; }

        [JsonProperty(PropertyName = "RingType")]
        public string RingType { get; private set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; private set; }

        public YarnNumberDocumentDto(YarnNumberDocument ringDocument)
        {
            Id = ringDocument.Identity;
            Code = ringDocument.Code;
            Number = ringDocument.Number;
            RingType = ringDocument.RingType;
            Description = ringDocument.Description;
        }
    }
}
