using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos
{
    public class ConstructionDocumentDto
    {
        public ConstructionDocumentDto(ConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            AmountOfWarp = constructionDocument.AmountOfWarp;
            AmountofWeft = constructionDocument.AmountOfWeft;
            Width = constructionDocument.Width;
            WovenType = constructionDocument.WovenType;
            WarpType = constructionDocument.WarpType;
            WeftType = constructionDocument.WeftType;
            TotalYarn = constructionDocument.TotalYarn;
            MaterialType = constructionDocument.MaterialType;
            Warps = constructionDocument.Warps;
            Wefts = constructionDocument.Wefts;
        }

        public Guid Id { get; }
        public int AmountOfWarp { get; }
        public int AmountofWeft { get; }
        public int Width { get; }
        public string WovenType { get; }
        public string WarpType { get; }
        public string WeftType { get; }
        public double TotalYarn { get; }
        public MaterialType MaterialType { get; }
        public IReadOnlyCollection<ConstructionDetail> Warps { get; }
        public IReadOnlyCollection<ConstructionDetail> Wefts { get; }
    }
}
