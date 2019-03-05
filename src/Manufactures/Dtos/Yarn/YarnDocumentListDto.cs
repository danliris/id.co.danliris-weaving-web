using Manufactures.Domain.Yarns;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Yarn
{
    public class YarnDocumentListDto
    {
        [JsonProperty]
        public Guid Id { get; }

        [JsonProperty]
        public string Code { get; }

        [JsonProperty]
        public string Name { get; }

        public YarnDocumentListDto(YarnDocument document)
        {
            Id = document.Identity;
            Code = document.Code;
            Name = document.Name;
        }
    }
}
