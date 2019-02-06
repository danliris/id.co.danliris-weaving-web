using Manufactures.Domain.Construction;
using System;

namespace Manufactures.Dtos
{
    public class ConstructionDocumentDto
    {
        public ConstructionDocumentDto(ConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalYarn = constructionDocument.TotalYarn;
            Date = constructionDocument.Date.ToString("MMMM dd, yyyy");
        }

        public Guid Id { get; }
        public string ConstructionNumber { get; }
        public string Date { get; }
        public double TotalYarn { get; }
        
    }
}
