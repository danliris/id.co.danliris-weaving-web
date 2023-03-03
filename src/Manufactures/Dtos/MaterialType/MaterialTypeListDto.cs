using Manufactures.Domain.Materials;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.MaterialType
{
    public class MaterialTypeListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        public MaterialTypeListDto(MaterialTypeDocument materialType)
        {
            Id = materialType.Identity;
            Code = materialType.Code;
            Name = materialType.Name;
        }
    }
}
