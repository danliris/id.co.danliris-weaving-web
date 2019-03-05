using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Yarns;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Yarn
{
    public class YarnDocumentDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "Code")]
        public string Code { get; }

        [JsonProperty(propertyName: "Name")]
        public string Name { get; }

        [JsonProperty(propertyName: "Tags")]
        public string Tags { get; }

        [JsonProperty(propertyName: "MaterialTypeDocument")]
        public MaterialTypeValueObject MaterialTypeDocument { get; }

        [JsonProperty(propertyName: "YarnNumberDocument")]
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
