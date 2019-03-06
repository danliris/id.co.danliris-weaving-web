using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Construction
{
    public class ConstructionDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "Date")]
        public string Date { get; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; }

        public ConstructionDocumentDto(ConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalYarn = constructionDocument.TotalYarn;
            Date = constructionDocument.Date.ToString("MMMM dd, yyyy");
        }
    }
}
