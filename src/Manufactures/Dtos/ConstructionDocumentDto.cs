using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.Yarns.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos
{
    public class ConstructionDocumentDto
    {
        public ConstructionDocumentDto(ConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalYarn = constructionDocument.TotalYarn;
            Date = constructionDocument.Date;
        }

        public Guid Id { get; }
        public string ConstructionNumber { get; }
        public DateTimeOffset Date { get; }
        public double TotalYarn { get; }
        
    }
}
