using Manufactures.Domain.FabricConstructions;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.FabricConstructions
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

        public FabricConstructionDocumentDto(FabricConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalYarn = constructionDocument.TotalYarn;
            Date = constructionDocument.Date.ToString("MMMM dd, yyyy");
        }
    }
}
