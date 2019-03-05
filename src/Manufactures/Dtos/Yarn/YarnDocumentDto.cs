using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Yarns;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Yarn
{
    public class YarnDocumentDto
    {
        [JsonProperty]
        public Guid Id { get; }

        [JsonProperty]
        public string Code { get; }

        [JsonProperty]
        public string Name { get; }

        [JsonProperty]
        public string Tags { get; }

        [JsonProperty]
        public MaterialTypeValueObject MaterialTypeDocument { get; }

        [JsonProperty]
        public YarnNumberValueObject YarnNumberDocument { get; }

        public YarnDocumentDto(YarnDocument yarn, 
                               MaterialTypeValueObject materialType, 
                               YarnNumberValueObject yarnNumber)
        {
            Id = yarn.Identity;
            Code = yarn.Code;
            Name = yarn.Name;
            Tags = yarn.Tags;
            MaterialTypeDocument = materialType;
            YarnNumberDocument = yarnNumber;
        }
    }
}
