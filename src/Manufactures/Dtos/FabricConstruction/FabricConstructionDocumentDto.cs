using Manufactures.Domain.FabricConstruction;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.FabricConstruction
{
    public class FabricConstructionDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "Date")]
        public string Date { get; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; }

        public FabricConstructionDocumentDto(ConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalYarn = constructionDocument.TotalYarn;
            Date = constructionDocument.Date.ToString("MMMM dd, yyyy");
        }
    }
}
