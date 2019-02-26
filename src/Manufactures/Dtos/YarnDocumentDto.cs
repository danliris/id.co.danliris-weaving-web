using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Yarns;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos
{
    public class YarnDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; }

        [JsonProperty(PropertyName = "Tags")]
        public string Tags { get; }

        [JsonProperty(PropertyName = "MaterialTypeDocument")]
        public MaterialTypeValueObject MaterialTypeDocument { get; }

        [JsonProperty(PropertyName = "RingDocument")]
        public RingDocumentValueObject RingDocument { get; }

        public YarnDocumentDto(YarnDocument document)
        {
            Id = document.Identity;
            Code = document.Code;
            Name = document.Name;
            Tags = document.Tags;
            MaterialTypeDocument = document.MaterialTypeDocument;
            RingDocument = document.RingDocument;
        }
    }
}
