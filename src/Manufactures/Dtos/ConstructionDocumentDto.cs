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
            AmountOfWarp = constructionDocument.AmountOfWarp;
            AmountOfWeft = constructionDocument.AmountOfWeft;
            Width = constructionDocument.Width;
            WovenType = constructionDocument.WovenType;
            WarpType = constructionDocument.WarpType;
            WeftType = constructionDocument.WeftType;
            TotalYarn = constructionDocument.TotalYarn;
            MaterialTypeId = constructionDocument.MaterialType;

            var warps = new List<Warp>();
            var wefts = new List<Weft>();

            foreach (var constructionDetail in constructionDocument.ConstructionDetails)
            {
                if (constructionDetail.Detail.Contains(Constants.WARP))
                {
                    var warpObj = new Warp(constructionDetail.Identity,
                                           constructionDetail.Quantity,
                                           constructionDetail.Information,
                                           constructionDetail.Yarn.Deserialize<Yarn>(),
                                           constructionDetail.Detail);
                    warps.Add(warpObj);

                }
                else if (constructionDetail.Detail.Contains(Constants.WEFT))
                {
                    var weftObj = new Weft(constructionDetail.Identity,
                                           constructionDetail.Quantity,
                                           constructionDetail.Information,
                                           constructionDetail.Yarn.Deserialize<Yarn>(),
                                           constructionDetail.Detail);

                    wefts.Add(weftObj);
                }
            }

            Warps = warps;
            Wefts = wefts;
        }

        public Guid Id { get; }
        public string ConstructionNumber { get; }
        public int AmountOfWarp { get; }
        public int AmountOfWeft { get; }
        public int Width { get; }
        public string WovenType { get; }
        public string WarpType { get; }
        public string WeftType { get; }
        public double TotalYarn { get; }
        public MaterialTypeDocumentValueObject MaterialTypeId { get; }
        public IReadOnlyCollection<Warp> Warps { get; }
        public IReadOnlyCollection<Weft> Wefts { get; }
    }
}
