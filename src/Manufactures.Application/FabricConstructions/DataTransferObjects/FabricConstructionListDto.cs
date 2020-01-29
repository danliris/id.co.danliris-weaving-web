using Manufactures.Domain.FabricConstructions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.FabricConstructions.DataTransferObjects
{
    public class FabricConstructionListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateCreated")]
        public DateTimeOffset DateCreated { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; }

        public FabricConstructionListDto(FabricConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            DateCreated = constructionDocument.AuditTrail.CreatedDate;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalYarn = constructionDocument.TotalYarn;
        }
    }
}
