using Manufactures.Domain.Yarns;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Yarn
{
    public class YarnDocumentListDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "Code")]
        public string Code { get; }

        [JsonProperty(propertyName: "Name")]
        public string Name { get; }

        public YarnDocumentListDto(YarnDocument document)
        {
            Id = document.Identity;
            Code = document.Code;
            Name = document.Name;
        }
    }
}
