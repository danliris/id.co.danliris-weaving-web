using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Entity;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.FabricConstructions.DataTransferObjects
{
    public class ConstructionYarnDetailDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "YarnId")]
        public Guid YarnId { get; }

        [JsonProperty(PropertyName = "Code")]
        public string Code{ get; }

        [JsonProperty(PropertyName = "YarnName")]
        public string YarnName { get; }

        [JsonProperty(PropertyName = "Quantity")]
        public double Quantity { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; }

        [JsonProperty(PropertyName = "FabricConstructionDocumentId")]
        public Guid FabricConstructionDocumentId { get; }

        public ConstructionYarnDetailDto(FabricConstructionDocument fabricConstructionDocument, ConstructionYarnDetail constructionYarnDetail, string yarnCode, string yarnName)
        {
            Id = constructionYarnDetail.Identity;
            YarnId = constructionYarnDetail.YarnId.Value;
            Code= yarnCode;
            YarnName = yarnName;
            Quantity = constructionYarnDetail.Quantity;
            Information = constructionYarnDetail.Information;
            Type = constructionYarnDetail.Type;
            FabricConstructionDocumentId = fabricConstructionDocument.Identity;
        }
    }
}
