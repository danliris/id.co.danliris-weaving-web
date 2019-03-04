using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Dtos.MaterialType
{
    public class MaterialTypeDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "RingDocuments")]
        public List<YarnNumberValueObject> RingDocuments { get; private set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; private set; }

        public MaterialTypeDocumentDto(MaterialTypeDocument materialType)
        {
            Id = materialType.Identity;
            Code = materialType.Code;
            Name = materialType.Name;
            RingDocuments = materialType.RingDocuments.ToList();
            Description = materialType.Description;
         }
    }
}
